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
	public class AddAndSelectSingleNode : DiagramAction
	{
		Node m_node;

		public AddAndSelectSingleNode( Diagram diagram, Node node )
			: base( "Add node to diagram", diagram )
		{
			m_node = node;

			AddHistoryOperation( HistoryOperation.STORE_ON_SUCCESS );
		}

		protected override ActionResult OnExecute()
		{
			Diagram.AddNode( m_node );
			m_node.Select();

			return ActionResult.SUCCESS;
		}

		protected override ActionResult OnUndo()
		{
			// We can safely remove it without taking into consideration connections
			// as nodes are created without any connection.

			m_node.Select( false );
			Diagram.RemoveNode( m_node );

			return ActionResult.SUCCESS;
		}

		protected override ActionResult OnRedo()
		{
			return OnExecute();
		}
	}
}
