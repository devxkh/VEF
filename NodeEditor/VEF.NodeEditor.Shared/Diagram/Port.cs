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
using System.IO;
using System.Diagnostics;

namespace Toothrot.Diagram
{
	public enum HPortAlign
	{
		LEFT,
		RIGHT,
		CENTRE
	}

	public enum VPortAlign
	{
		TOP,
		BOTTOM,
		CENTRE
	}

	public class Port
	{
		public static int DEFAULT_WIDTH = 17;
		public static int DEFAULT_HEIGHT = 17;

		String m_name;

		Node m_node;
		IElementParent m_parent;

		PointF m_relativePositionToParent;
		bool m_relativePositionIsPixels;
		HPortAlign m_hPortAlign;
		VPortAlign m_vPortAlign;

		bool m_allowMultipleConnections;
		
		Rectangle m_dimensions;
		Point m_location;

		List< Port > m_connections;

		public String Name
		{
			get { return m_name; }
		}

		public Node Node
		{
			get { return m_node; }
		}

		public Point Location
		{
			get { return m_location; }
			set { m_location = value; }
		}

		public Size Size
		{
			get { return m_dimensions.Size; }
			set { m_dimensions.Size = value; }
		}

		public int Width
		{
			get { return Size.Width; }
		}

		protected float HalfWidth
		{
			get { return Width * 0.5f; }
		}

		public int Height
		{
			get { return Size.Height; }
		}

		protected float HalfHeight
		{
			get { return Height * 0.5f; }
		}

		public Rectangle Dimensions
		{
			get { return m_dimensions; }
		}

		public int X
		{
			get { return m_dimensions.Left; }
		}

		public int Y
		{
			get { return m_dimensions.Y; }
		}

		public bool AllowMultipleConnections
		{
			get { return m_allowMultipleConnections; }
			set { m_allowMultipleConnections = value; }
		}

		public IEnumerable< Port > Connections
		{
			get { return m_connections; }
		}

		public bool HasConnections
		{
			get { return ( m_connections.Count != 0 ); }
		}

		public IElementParent Parent
		{
			get
			{
				return m_parent;
			}
			set
			{
				if ( m_parent == value )
				{
					return;
				}

				if ( m_parent != null )
				{
					m_parent.LocationChanged -= new EventHandler( Parent_LocationChanged );
					m_parent.SizeChanged -= new EventHandler( Parent_SizeChanged );
				}

				m_parent = value;

				if ( m_parent != null )
				{
					m_parent.LocationChanged += new EventHandler( Parent_LocationChanged );
					m_parent.SizeChanged += new EventHandler( Parent_SizeChanged );
				}

				UpdateLocation();
			}
		}

		public PointF RelativePositionToParent
		{
			get { return m_relativePositionToParent; }
			set
			{ 
				m_relativePositionToParent = value;
				UpdateLocation();
			}
		}

		public bool RelativePositionIsPixels
		{
			get { return m_relativePositionIsPixels; }
			set
			{ 
				m_relativePositionIsPixels = value;
				UpdateLocation();
			}
		}

		public HPortAlign HorizontalPortAlign
		{
			get { return m_hPortAlign; }
			set
			{ 
				m_hPortAlign = value;
				UpdateLocation();
			}
		}

		public VPortAlign VerticalPortAlign
		{
			get { return m_vPortAlign; }
			set 
			{ 
				m_vPortAlign = value;
				UpdateLocation();
			}
		}

		public int RelativePixelsX
		{
			get
			{
				float toPixelsConversionFactor = ( RelativePositionIsPixels ) ? 1 : Parent.Width;
				if ( HorizontalPortAlign == HPortAlign.LEFT )
				{
					return ( int )( RelativePositionToParent.X * toPixelsConversionFactor );	
				}
				else if ( HorizontalPortAlign == HPortAlign.RIGHT )
				{
					return Parent.Width - ( int ) ( RelativePositionToParent.X * toPixelsConversionFactor );
				}
				else // HorizontalPortAlign == HPortAlign.CENTRE
				{
					return ( Parent.Width / 2 ) + ( int ) ( RelativePositionToParent.X * toPixelsConversionFactor );
				}
			}
		}

		public int RelativePixelsY
		{
			get
			{
				float toPixelsConversionFactor = ( RelativePositionIsPixels ) ? 1 : Parent.Height;
				if ( VerticalPortAlign == VPortAlign.TOP )
				{
					return ( int )( RelativePositionToParent.Y * toPixelsConversionFactor );
				}
				else if ( VerticalPortAlign == VPortAlign.BOTTOM )
				{
					return Parent.Height - ( int ) ( RelativePositionToParent.Y * toPixelsConversionFactor );
				}
				else// VerticalPortAlign == VPortAlign.CENTRE
				{
					return ( Parent.Height / 2 ) + ( int ) ( RelativePositionToParent.Y * toPixelsConversionFactor );
				}
			}
		} 

		public Port( String name, Node node )
		{
			m_name = name;

			m_node = node;
			m_parent = null;
			Parent = node;

			m_relativePositionToParent = new PointF();
			m_relativePositionIsPixels = false;
			m_hPortAlign = HPortAlign.LEFT;
			m_vPortAlign = VPortAlign.TOP;

			m_allowMultipleConnections = false;

			m_dimensions = new Rectangle();

			m_dimensions.Size = new Size( DEFAULT_WIDTH, DEFAULT_HEIGHT );
			
			UpdateLocation();

			m_connections = new List< Port >();

			Node.AddPort( this );
		}

		void Parent_SizeChanged( object sender, EventArgs e )
		{
			UpdateLocation();
		}

		void Parent_LocationChanged( object sender, EventArgs e )
		{
			UpdateLocation();
		}

		protected virtual void UpdateLocation()
		{
			int x = Parent.Left + RelativePixelsX;
			int y = Parent.Top + RelativePixelsY;

			m_dimensions.Location = new Point( x - ( int )( HalfWidth ), y - ( int )( HalfHeight ) );
			m_location = new Point( x, y );
		}

		public void AddConnection( Port other )
		{
			Debug.Assert( ! IsConnectedTo( other ) );
			if ( IsConnectedTo( other ) )
			{
				return;
			}

			m_connections.Add( other );
		}

		public void RemoveConnection( Port other )
		{
			Debug.Assert( IsConnectedTo( other ) );
			if ( ! IsConnectedTo( other ) )
			{
				return;
			}

			m_connections.Remove( other );
		}

		// Warning: It just clears connections from this ports to others, it does nothing to make
		// sure no other ports are connected to this port.
		// Use a PortConnector class to make sure connections are cleanly added or removed.
		public void ClearConnections()
		{
			m_connections.Clear();
		}

		public bool IntersectsWith( Point p )
		{
			Rectangle pixelRectangle = new Rectangle( p, new Size( 1, 1 ) );
			return Dimensions.IntersectsWith( pixelRectangle );
		}

		public virtual void Paint( Graphics g, Font f )
		{
			g.DrawRectangle( Pens.GreenYellow, Dimensions );
		}

		public bool IsConnectedTo( Port portTo )
		{
			return m_connections.Contains( portTo );
		}
	}
}
