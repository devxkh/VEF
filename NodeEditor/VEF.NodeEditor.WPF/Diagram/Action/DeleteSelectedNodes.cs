/*
Toothrot .Net control library 
Copyright (C) 2008 Pau Novau Lebrato

This library is free software; you can redistribute it and/or modify it under 
the terms of the GNU Lesser General Public License as published by the Free Software 
Foundation; either version 2.1 of the License, or (at your option) any later 
version.

This program is distributed in the hope that it will be useful, but WITHOUT 
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with 
this program; if not, write to the Free Software Foundation, Inc., 59 Temple 
Place - Suite 330, Boston, MA 02111-1307, USA
*/

using System;
using System.Collections.Generic;
using System.Text;
using Toothrot.Action;

namespace Toothrot.Diagram.Action
{
	public class DeleteSelectedNodes : DiagramAction
	{
		List< Node > m_nodes;
		List< DisconnectPort > m_disconnectPortActions;

		PortConnector m_connector;

		public DeleteSelectedNodes( Diagram diagram, PortConnector connector )
			: base( "Delete Selected Nodes", diagram )
		{
			m_nodes = new List< Node >();
			m_connector = connector;

			AddHistoryOperation( HistoryOperation.STORE_ON_SUCCESS );
		}


		public void AddNode( Node node )
		{
			if ( node == null )
			{
				return;
			}

			if ( Executed )
			{
				return;
			}

			if ( ! CanDeleteNode( node ) )
			{
				return;
			}

			m_nodes.Add( node );
		}

		
		
		private bool CanDeleteNode( Node node )
		{
			return node.Deletable;
		}



		public void AddNodes( IEnumerable< Node > nodes )
		{
			if ( nodes == null )
			{
				return;
			}

			if ( Executed )
			{
				return;
			}

			foreach ( Node node in nodes )
			{
				AddNode( node );
			}
		}



		protected override ActionResult OnExecute()
		{
			if ( m_nodes.Count == 0 )
			{
				FailureReason = "No nodes requested for deletion";
				return ActionResult.FAILURE;
			}

			m_disconnectPortActions = new List< DisconnectPort >();

			foreach ( Node node in m_nodes )
			{
				foreach ( Port port in node.Ports )
				{
					DisconnectPort disconnectPortAction = new DisconnectPort( Diagram, m_connector, port );
					disconnectPortAction.Execute();
					m_disconnectPortActions.Add( disconnectPortAction );
				}
				Diagram.RemoveNode( node );
			}

			return ActionResult.SUCCESS;
		}



		protected override ActionResult OnUndo()
		{
			foreach ( Node node in m_nodes )
			{
				Diagram.AddNode( node );
			}

			foreach ( DisconnectPort disconnectPortAction in m_disconnectPortActions )
			{
				disconnectPortAction.Undo();
			}

			return ActionResult.SUCCESS;
		}



		protected override ActionResult OnRedo()
		{
			return OnExecute();
		}

	}
}
