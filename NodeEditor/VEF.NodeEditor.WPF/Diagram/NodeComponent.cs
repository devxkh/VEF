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
using System.Windows.Input;

namespace Toothrot.Diagram
{
	public class NodeComponent : INodeComponent, IElementParent
	{
		int m_offsetX;		
		int m_offsetY;
		Point m_location;
		Size m_size;
		Node m_node;
		IElementParent m_parent;

		//Padding m_margins;

        //public Padding Margins
        //{
        //    get { return m_margins; }
        //    set
        //    {
        //        if ( m_margins == value )
        //        {
        //            return;
        //        }

        //        m_margins = value;

        //        CallSizeChangedEvent();
        //    }
        //}

		public event EventHandler SizeChanged;
		public event EventHandler LocationChanged;
		

		public int OffsetX
		{
			get { return m_offsetX; }
			set { m_offsetX = value; }
		}

		public int OffsetY
		{
			get { return m_offsetY; }
			set { m_offsetY = value; }
		}

		public Point Location
		{
			get { return m_location; }
			set
			{ 
				if ( m_location == value )
				{
					return;
				}

				m_location = value;

				if ( LocationChanged != null )
				{
					LocationChanged( this, null );
				}
			}
		}

		public virtual Size Size
		{
			get { return m_size; }
			set
			{
				if ( m_size == value )
				{
					return;
				}

				m_size = value;

				CallSizeChangedEvent();
			}
		}

		protected void CallSizeChangedEvent()
		{
			if ( SizeChanged != null )
			{
				SizeChanged( this, null );
			}
		}

		public int Width
		{
			get { return Size.Width; }
		}

		public int Height
		{
			get { return Size.Height; }
		}

		public int Left
		{
			get { return Location.X; }
		}

		public int Top
		{
			get { return Location.Y; }
		}

		public Node Node
		{
			get { return m_node; }
			set
			{
				if ( m_node == value )
				{
					return;
				}

				if ( m_node != null )
				{
					m_node.LocationChanged -= new EventHandler( Node_LocationChanged );
				}

				m_node = value;
				UpdateLocation();

				if ( m_node != null )
				{
					m_node.LocationChanged += new EventHandler( Node_LocationChanged );
				}
			}
		}

		public IElementParent Parent
		{
			get
			{
				if ( m_parent == null )
				{
					return Node;
				}

				return m_parent;
			}

			set
			{
				m_parent = value;
			}
		}

		void Node_LocationChanged( object sender, EventArgs e )
		{
			UpdateLocation();
		}

		public NodeComponent()
		{
			m_parent = null;
			m_node = null;
			m_offsetX = 0;
			m_offsetY = 0;
			m_location = new Point();
			m_size = new Size();
		//	m_margins = new Padding( 10, 4, 10, 3 );
		}

		public void UpdateLocation()
		{
			if ( Node == null )
			{
				return;
			}

			Location = new Point( Parent.Left + OffsetX, Parent.Top + OffsetY );
		}


		public virtual void BringToFront()
		{
		}

        //public virtual void Register( DiagramControl diagramControl )
        //{

        //}

		public virtual void Unregister()
		{

		}

		public virtual void Draw( Graphics g, Font f )
		{

		}


		public virtual bool MouseDownEvent( MouseEventArgs e )
		{
			return false;
		}

		public virtual bool MouseMoveEvent( MouseEventArgs e )
		{
			return false;
		}

		public virtual bool MouseExitEvent( MouseEventArgs e )
		{
			return false;
		}

		public virtual bool MouseEnterEvent( MouseEventArgs e )
		{
			return false;
		}

		public Point ConvertPointToLocalCoordinates( Point PointInGlobalCoordinates )
		{
			return new Point( PointInGlobalCoordinates.X - Left, PointInGlobalCoordinates.Y - Top );
		}

		public bool IntersectsWith( Point location )
		{
			Rectangle pixelRectangle = new Rectangle( location, new Size( 1, 1 ) );
			Rectangle dimensions = new Rectangle( Location, Size );
			return dimensions.IntersectsWith( pixelRectangle );
		}



		
	}
}
