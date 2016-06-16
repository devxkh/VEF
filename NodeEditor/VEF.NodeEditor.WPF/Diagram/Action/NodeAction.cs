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

namespace Toothrot.Diagram.Action
{
	public abstract class NodeAction : Toothrot.Action.Action
	{
		Node m_node;

		protected Node Node
		{
			get { return m_node; }
		}

		public NodeAction( string name, Node node )
			: base( name )
		{
			m_node = node;
		}
	}
}
