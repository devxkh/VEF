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
	public enum NodeSelectionOperation
	{
		SELECT,
		DESELECT,
		TOGGLE
	}

	public class ModifySelectedNodes : DiagramAction
	{
		class NodeSelectionInfo
		{
			Node m_node;
			NodeSelectionOperation m_operation;

			public Node Node
			{
				get { return m_node; }
			}

			public NodeSelectionOperation Operation
			{
				get { return m_operation; }
			}

			public NodeSelectionInfo( Node node, NodeSelectionOperation operation )
			{
				m_node = node;
				m_operation = operation;
			}
		}

		List< NodeSelectionInfo > m_nodes;

		public ModifySelectedNodes( Diagram diagram )
			: base( "Modify Selected Nodes", diagram )
		{
			m_nodes = new List< NodeSelectionInfo >();

			AddHistoryOperation( HistoryOperation.STORE_ON_SUCCESS );
			AddHistoryOperation( HistoryOperation.PASS_THROUGH );
		}

		bool ValidOperation( Node node, NodeSelectionOperation operation )
		{
			if ( operation == NodeSelectionOperation.SELECT )
			{
				if ( node.Selected )
				{
					return false;
				}
			}

			if ( operation == NodeSelectionOperation.DESELECT )
			{
				if ( ! node.Selected )
				{
					return false;
				}
			}

			return true;
		}

		NodeSelectionOperation GetInverseOperation( NodeSelectionOperation operation )
		{
			switch ( operation )
			{
				case NodeSelectionOperation.SELECT:
					return NodeSelectionOperation.DESELECT;

				case NodeSelectionOperation.DESELECT:
					return NodeSelectionOperation.SELECT;

				case NodeSelectionOperation.TOGGLE:
					return NodeSelectionOperation.TOGGLE;
				
				default:
					return NodeSelectionOperation.TOGGLE;
			}
			
		}

		public void AddNode( Node node, NodeSelectionOperation operation )
		{
			if ( node == null )
			{
				return;
			}

			if ( Executed )
			{
				return;
			}

			if ( ! ValidOperation( node, operation ) )
			{
				return;
			}

			m_nodes.Add( new NodeSelectionInfo( node, operation ) );
		}

		public void AddNodes( IEnumerable<Node> nodes, NodeSelectionOperation operation )
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
				AddNode( node, operation );
			}
		}

		void ChangeNodeSelection( Node node, NodeSelectionOperation operation )
		{
			if ( operation == NodeSelectionOperation.SELECT )
			{
				node.Select();
				return;
			}

			if ( operation == NodeSelectionOperation.DESELECT )
			{
				node.Deselect();
				return;
			}

			if ( operation == NodeSelectionOperation.TOGGLE )
			{
				node.ToggleSelection();
				return;
			}
		}

		override protected ActionResult OnExecute()
		{
			if ( m_nodes.Count == 0 )
			{
				FailureReason = "No nodes requested for selection change";
				return ActionResult.FAILURE;
			}

			foreach ( NodeSelectionInfo nodeInfo in m_nodes )
			{
				ChangeNodeSelection( nodeInfo.Node, nodeInfo.Operation );
			}

			return ActionResult.SUCCESS;
		}

		override protected ActionResult OnUndo()
		{
			foreach ( NodeSelectionInfo nodeInfo in ReverseIterator( m_nodes ) )
			{
				NodeSelectionOperation inverseOperation = GetInverseOperation( nodeInfo.Operation );
				ChangeNodeSelection( nodeInfo.Node, inverseOperation );
			}

			return ActionResult.SUCCESS;
		}

		override protected ActionResult OnRedo()
		{
			return OnExecute();
		}

		static IEnumerable< T > ReverseIterator< T >( IList< T > list )
		{
			for ( int i = list.Count - 1; 0 <= i; i-- )
			{
				yield return list[ i ];
			}
		}
	}
}
