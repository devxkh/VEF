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
using System.Drawing;
using System.Diagnostics;

namespace Toothrot.Diagram
{
	public class Diagram
	{
		List< Node > m_nodes;
		List< Node > m_selectedNodes;

		Rectangle m_selectionExtents;

		public event EventHandler< NodeEventArgs > NodeAdded;
		public event EventHandler< NodeEventArgs > NodeRemoved;

		public IList< Node > Nodes
		{
			get { return m_nodes; }
		}

		public IEnumerable< Node > SelectedNodes
		{
			get { return m_selectedNodes; }
		}

		public Rectangle SelectionExtents
		{
			get { return m_selectionExtents; }
		}

		public Diagram()
		{
			m_nodes = new List< Node >();
			m_selectedNodes = new List< Node >();
			m_selectionExtents = new Rectangle();
		}

		public void AddNode( Node node )
		{
			if ( m_nodes.Contains( node ) )
			{
				return;
			}

			m_nodes.Add( node );
			node.SelectedChanged += new EventHandler( NodeSelectionChanged );

			if ( node.Selected )
			{
				m_selectedNodes.Add( node );
				UpdateSelectionExtents();
			}

			if ( NodeAdded != null )
			{
				NodeAdded( this, new NodeEventArgs( node ) );
			}
		}

		public void RemoveNode( Node node )
		{
			if ( !m_nodes.Contains( node ) )
			{
				return;
			}

			m_nodes.Remove( node );

			node.SelectedChanged -= new EventHandler( NodeSelectionChanged );
			if ( node.Selected )
			{
				if ( m_selectedNodes.Contains( node ) )
				{
					m_selectedNodes.Remove( node );
					UpdateSelectionExtents();
				}
			}


			if ( NodeRemoved != null )
			{
				NodeRemoved( this, new NodeEventArgs( node ) );
			}
		}

		public void RemoveAllNodes()
		{
			List< Node > scheduledForRemoval = new List< Node >( Nodes );
			foreach ( Node node in scheduledForRemoval )
			{
				RemoveNode( node );
			}
		}

		void NodeSelectionChanged( object sender, EventArgs e )
		{
			Node node = ( Node ) sender;
			if ( node == null )
			{
				return;
			}

			if ( node.Selected )
			{
				m_selectedNodes.Add( node );
				BringNodeToFront( node );
			}
			else
			{
				m_selectedNodes.Remove( node );
			}

			UpdateSelectionExtents();
		}

		void UpdateSelectionExtents()
		{
			if ( m_selectedNodes.Count == 0 )
			{
				m_selectionExtents = new Rectangle();
				return;
			}

			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;

			foreach ( Node node in SelectedNodes )
			{
				minX = Math.Min( node.Left, minX );
				minY = Math.Min( node.Top, minY );
				maxX = Math.Max( node.Right, maxX );
				maxY = Math.Max( node.Bottom, maxY );
			}

			int width = maxX - minX;
			int height = maxY - minY;

			m_selectionExtents = new Rectangle( minX, minY, width, height );
		}

		public void MoveSelectedNodes( int deltaX, int deltaY )
		{
			foreach ( Node node in SelectedNodes )
			{
				node.Move( deltaX, deltaY );
			}

			m_selectionExtents.Offset( deltaX, deltaY );
		}

		void BringNodeToFront( Node node )
		{
			m_nodes.Remove( node );
			m_nodes.Add( node );
		}



		public virtual void Paint( Graphics g, Font f )
		{
			PaintConnections( g );
			PaintNodes( g, f );
		}

		protected virtual void PaintConnections( Graphics g )
		{
			Pen connectionPen = new Pen( Color.Black, 2 );
			foreach ( Node node in m_nodes )
			{
				node.PaintOutConnections( g, connectionPen );
			}
			connectionPen.Dispose();
		}

		protected virtual void PaintNodes( Graphics g, Font f )
		{
			foreach ( Node node in m_nodes )
			{
				node.Paint( g, f );
			}
		}

		public virtual void DrawFreeConnection( Graphics g, Port portFrom, Port portTo, Point fallbackEndPoint, bool validConnection )
		{
			Debug.Assert( portFrom != null );

			Node node = portFrom.Node;

			if ( validConnection )
			{
				Pen validConnectionPen = new Pen( Color.White, 2 );
				node.PaintConnection( g, validConnectionPen, portFrom, portTo );
			}
			else
			{
				Pen invalidConnectionPen = new Pen( Color.DarkRed, 2 );
				if ( portTo == null )
				{
					node.PaintConnection( g, invalidConnectionPen, portFrom, fallbackEndPoint );
				}
				else
				{
					node.PaintConnection( g, invalidConnectionPen, portFrom, portTo );
				}
			}
		}

		public virtual void DrawEndangeredConnections( Graphics g, Port portFrom, Port portTo, bool validConnection )
		{
			Pen endangeredConnectionPen = new Pen( Color.DarkGray, 2.5f );

			if ( ! validConnection )
			{
				if ( portTo == null )
				{
					// connection will be destroyed when trying to connect to a null port.
					DrawConnections( g, endangeredConnectionPen, portFrom );
				}
				return;
			}
			

			if ( ! portFrom.AllowMultipleConnections )
			{
				DrawConnections( g, endangeredConnectionPen, portFrom );
			}

			if ( ! portTo.AllowMultipleConnections )
			{
				DrawConnections( g, endangeredConnectionPen, portTo );
			}
		}

		protected virtual void DrawConnections( Graphics g, Pen pen, Port port )
		{
			Node node = port.Node;
			foreach ( Port otherPort in port.Connections)
			{
				node.PaintConnection( g, pen, port, otherPort );
			}
		}


		public Node GetNodeByLocation( Point location )
		{
			foreach ( Node node in ReverseIterator( m_nodes ) )
			{
				if ( node.IntersectsWith( location ) )
				{
					return node;
				}
			}

			return null;
		}

		public Node GetNodeByLocation( int x, int y )
		{
			return GetNodeByLocation( new Point( x, y ) );
		}

		public Port GetPortByLocation( Point location )
		{
			// TODO: could be improved to check all nodes in case a node is occluding another node.

			Node node = GetNodeByLocation( location );
			
			bool nodeFoundAtLocation = ( node != null );
			if ( ! nodeFoundAtLocation )
			{
				return null;
			}

			Port port = node.GetPortByLocation( location );
			return port;
		}

		public Port GetPortByLocation( int x, int y )
		{
			return GetPortByLocation( new Point( x, y ) );
		}


		public NodeComponent GetComponentByLocation( Point location )
		{
			Node node = GetNodeByLocation( location );

			bool nodeFoundAtLocation = ( node != null );
			if ( ! nodeFoundAtLocation )
			{
				return null;
			}

			NodeComponent component = node.getComponentByLocation( location );
			return component;
		}

		public NodeComponent GetComponentByLocation( int x, int y )
		{
			return GetComponentByLocation( new Point( x, y ) );
		}

		static IEnumerable< T > ReverseIterator< T >( IList<T> list )
		{
			for ( int i = list.Count - 1; 0 <= i; i-- )
			{
				yield return list[ i ];
			}
		}

		public IEnumerable< Node > GetNodesByArea( Rectangle selectionArea )
		{
			List< Node > nodesInSelectionArea = new List< Node >();

			foreach ( Node node in m_nodes )
			{
				if ( node.IntersectsWith( selectionArea ) )
				{
					nodesInSelectionArea.Add( node );
				}
			}

			return nodesInSelectionArea;
		}



		
	}
}
