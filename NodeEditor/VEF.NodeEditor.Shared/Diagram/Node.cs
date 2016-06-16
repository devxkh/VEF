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
using System.Drawing.Drawing2D;
using Toothrot.Diagram.Action;

namespace Toothrot.Diagram
{
	public class Node : IElementParent
	{
		// Factory function that creates NodeAction instances when called.
		public delegate NodeAction NodeActionCreatorDelegate( Node node );



		#region Fields
		String m_name;


		bool m_selected;


		bool m_selectable;
		bool m_deletable;
		bool m_movable;


		Rectangle m_dimensions;
	//	Padding m_boundingMargins;

		// The bounding box is calculated from the dimensions and the padding properties.
		Rectangle m_boundingBox;


		Dictionary< String, Port > m_ports;


		List< INodeComponent > m_components;


		Dictionary<String, NodeActionCreatorDelegate> m_actions;
		#endregion


		#region Events
		public event EventHandler NameChanged;

		public event EventHandler SelectedChanged;

		public event EventHandler LocationChanged;
		public event EventHandler SizeChanged;

		public event EventHandler< PortEventArgs > PortAdded;
		public event EventHandler< PortEventArgs > PortRemoved;

		public event EventHandler< NodeComponentEventArgs > ComponentAdded;
		public event EventHandler< NodeComponentEventArgs > ComponentRemoved;
		#endregion


		#region Properties
		public String Name
		{
			get { return m_name; }
			set
			{
				if ( m_name == value )
				{
					return;
				}

				m_name = value;

				if ( NameChanged != null )
				{
					NameChanged( this, null );
				}
			}
		}


		public bool Selected
		{
			get { return m_selected; }
		}


		public bool Selectable
		{
			get { return m_selectable; }
			protected set { m_selectable = value; }
		}

		public bool Deletable
		{
			get { return m_deletable; }
			protected set { m_deletable = value; }
		}

		public bool Movable
		{
			get { return m_movable; }
			protected set { m_movable = value; }
		}

		
		// Location, Size, Left, Right, Top, Bottom, Width and Height are all extracted
		// from the dimensions properties.
		public Rectangle Dimensions
		{
			get { return m_dimensions; }
		}

		public Point Location
		{
			get { return m_dimensions.Location; }
		}

		public Size Size
		{
			get { return m_dimensions.Size; }
		}

		public int Left
		{
			get { return m_dimensions.Left; }
		}

		public int Right
		{
			get { return m_dimensions.Right; }
		}

		public int Top
		{
			get { return m_dimensions.Top; }
		}

		public int Bottom
		{
			get { return m_dimensions.Bottom; }
		}

		public int Width
		{
			get { return m_dimensions.Width; }
		}

		public int Height
		{
			get { return m_dimensions.Height; }
		}


		public Rectangle BoundingBox
		{
			get { return m_boundingBox; }
			private set { m_boundingBox = value; }
		}


        //public Padding BoundingMargins
        //{
        //    get { return m_boundingMargins; }
        //    protected set
        //    {
        //        m_boundingMargins = value;
        //        RecalculateBoundingBox();
        //    }
        //}



		public IEnumerable< Port > Ports
		{
			get { return m_ports.Values; }
		}



		public IEnumerable< INodeComponent > NodeComponents
		{
			get { return m_components; }
		}



		public IEnumerable< String > ActionNames
		{
			get { return m_actions.Keys; }
		}
		#endregion



		public Node()
		{
			initialiseDefaultValues();
		}


		public Node( String name )
		{
			initialiseDefaultValues();
			m_name = name;
		}


		private void initialiseDefaultValues()
		{
			m_name = "Unnamed";
			
			m_selected = false;

			m_selectable = true;
			m_deletable = true;
			m_movable = true;

			m_dimensions = new Rectangle( new Point( 0, 0 ), new Size( 64, 64 ) );
		//	m_boundingMargins = new Padding( 10, 3, 10, 3 );
			RecalculateBoundingBox();
			
			m_ports = new Dictionary< String, Port >();

			m_components = new List< INodeComponent >();

			m_actions = new Dictionary< String, NodeActionCreatorDelegate >();


			NameChanged += new EventHandler( Node_NameChanged );
			
			SizeChanged += new EventHandler( Node_SizeChanged );

			ComponentAdded += new EventHandler< NodeComponentEventArgs >( Node_ComponentAdded );
			ComponentRemoved += new EventHandler< NodeComponentEventArgs >( Node_ComponentRemoved );
		}




		void Node_NameChanged( object sender, EventArgs e )
		{
			RecalculateSizeAndLayout();
		}



		void Node_SizeChanged( object sender, EventArgs e )
		{
			RecalculateBoundingBox();
		}

		void RecalculateBoundingBox()
		{
		//	BoundingBox = new Rectangle( Left - BoundingMargins.Left, Top - BoundingMargins.Top, Width + BoundingMargins.Horizontal, Height + BoundingMargins.Vertical );
		}
		

		

		
		void Node_ComponentAdded( object sender, NodeComponentEventArgs e )
		{
			RecalculateSizeAndLayout();
			e.NodeComponent.SizeChanged += new EventHandler( NodeComponent_SizeChanged );
		}

		
		void Node_ComponentRemoved( object sender, NodeComponentEventArgs e )
		{
			RecalculateSizeAndLayout();
			e.NodeComponent.SizeChanged -= new EventHandler( NodeComponent_SizeChanged );
		}

		void NodeComponent_SizeChanged( object sender, EventArgs e )
		{
			RecalculateSizeAndLayout();
		}

		protected virtual void RecalculateSizeAndLayout()
		{
            //int maxWidth = 32;
            //int maxMarginWidth = 32;

            //int totalHeight = 0;

            //foreach ( INodeComponent nc in NodeComponents )
            //{
            //    maxWidth = Math.Max( maxWidth, nc.Width );
            //    maxMarginWidth = Math.Max( maxMarginWidth, nc.Width + nc.Margins.Horizontal );
            ////todo?	totalHeight += nc.Margins.Top;

            //    //todo?	nc.OffsetX = nc.Margins.Left;
            //    nc.OffsetY = totalHeight;

            //    //todo?	totalHeight += nc.Height + nc.Margins.Bottom;

            //    nc.UpdateLocation();
            //}

            //foreach ( INodeComponent nc in NodeComponents )
            //{
            //    nc.Size = new Size( maxMarginWidth - nc.Margins.Horizontal, nc.Height );
            //}

            //totalHeight = Math.Max( 32, totalHeight );

            //m_dimensions.Size = new Size( maxMarginWidth, totalHeight );

            //CallSizeChangedEvent();
		}

		protected void CallSizeChangedEvent()
		{
			if ( SizeChanged != null )
			{
				SizeChanged( this, null );
			}
		}

		public void AddComponent( INodeComponent nc )
		{
			if ( nc == null )
			{
				return;
			}

			m_components.Add( nc );
			nc.Node = this;
			nc.SizeChanged +=new EventHandler( ChildNodeComponent_SizeChanged );

			if ( ComponentAdded != null )
			{
				ComponentAdded( this, new NodeComponentEventArgs( this, nc ) );
			}
		}

		public void RemoveComponent( INodeComponent nc )
		{
			if ( nc == null )
			{
				return;
			}

			if ( ! m_components.Contains( nc ) )
			{
				return;
			}

			m_components.Remove( nc );
			nc.Node = null;
			nc.SizeChanged -= new EventHandler( ChildNodeComponent_SizeChanged );

			if ( ComponentRemoved != null )
			{
				ComponentRemoved( this, new NodeComponentEventArgs( this, nc ) );
			}
		}

		private void ChildNodeComponent_SizeChanged( object sender, EventArgs args )
		{
			RecalculateSizeAndLayout();
		}

		public Port GetPortByName( String name )
		{
			if ( ! HasPort( name ) )
			{
				return null;
			}

			return m_ports[ name ];
		}

		public bool HasPort( String name )
		{
			return m_ports.ContainsKey( name );
		}

		public void AddPort( Port p )
		{
			Debug.Assert( p.Node == this );

			bool portWithThisNameExists = HasPort( p.Name );
			Debug.Assert( ! portWithThisNameExists );
			if ( portWithThisNameExists )
			{
				throw new Exception( "Cannot create port with name " + p.Name );
			}

			m_ports.Add( p.Name, p );

			if ( PortAdded != null )
			{
				PortAdded( this, new PortEventArgs( p ) );
			}
		}

		public void RemovePort( Port p )
		{
			Debug.Assert( p.Node == this );
			Debug.Assert( ! p.HasConnections );

			bool removed = m_ports.Remove( p.Name );
			if ( ! removed )
			{
				return;
			}

			if ( PortRemoved != null )
			{
				PortRemoved( this, new PortEventArgs( p ) );
			}
		}

		public void SetLocation( Point newLocation )
		{
			if ( ! Movable )
			{
				return;
			}

			m_dimensions = new Rectangle( newLocation, Size );
			RecalculateBoundingBox();

			if ( LocationChanged != null )
			{
				LocationChanged( this, new EventArgs() );
			}
		}

		public void SetLocation( int left, int top )
		{
			SetLocation( new Point( left, top ) );
		}

		public void Move( Point movementIncrement )
		{
			Move( movementIncrement.X, movementIncrement.Y );
		}

		public void Move( int deltaX, int deltaY )
		{
			SetLocation( Location.X + deltaX, Location.Y + deltaY );
		}

		public void Select( bool select )
		{
			if ( ! Selectable )
			{
				return;
			}

			if ( Selected == select )
			{
				return;
			}

			m_selected = select;

			if ( Selected )
			{
				foreach ( INodeComponent nc in NodeComponents )
				{
					nc.BringToFront();
				}
			}

			if ( SelectedChanged != null )
			{
				SelectedChanged( this, new EventArgs() );
			}
		}

		public void Select()
		{
			Select( true );
		}

		public void Deselect()
		{
			Select( false );
		}

		public void ToggleSelection()
		{
			Select( ! Selected );
		}

		public bool IntersectsWith( Point p )
		{
			Rectangle pixelRectangle = new Rectangle( p, new Size( 1, 1 ) );
			return IntersectsWith( pixelRectangle );
		}

		public bool IntersectsWith( Rectangle r )
		{
			return m_boundingBox.IntersectsWith( r );
		}

		public virtual void Paint( Graphics g, Font f )
		{

			//g.DrawRectangle( Pens.Sienna, m_boundingBox );
		
			

			
			//Rectangle r = new Rectangle( Left, Top, Width, Height );
			
			GUI.Drawing.FillShadowRoundRectangle( g, Color.Black, 0.7f, 3, m_dimensions, 5 );

			Brush brush = new LinearGradientBrush( m_dimensions, Color.LightGray, Color.White, LinearGradientMode.Vertical );

			GUI.Drawing.FillRoundRectangle( g, brush, m_dimensions, 5 );
			
			foreach ( NodeComponent nc in NodeComponents )
			{
				nc.Draw( g, f );
				//g.DrawRectangle( Pens.SpringGreen, new Rectangle( nc.Location, nc.Size ) );
			}

			

			if ( Selected )
			{
				GUI.Drawing.DrawRoundRectangle( g, new Pen( Color.Black, 3 ), m_dimensions, 5 );
				GUI.Drawing.DrawRoundRectangle( g, new Pen( Color.White, 2 ), m_dimensions, 5 );
			}
			else
			{
				GUI.Drawing.DrawRoundRectangle( g, new Pen( Color.Black, 2 ), m_dimensions, 5 );
			}

			foreach ( Port port in Ports )
			{
				port.Paint( g, f );
			}
		
		}

		public virtual void PaintOutConnections( Graphics g, Pen pen )
		{
			foreach ( Port port in Ports )
			{
				foreach ( Port otherPort in port.Connections )
				{
					PaintConnection( g, pen, port, otherPort );
				}
			}
		}

		public virtual void PaintConnection( Graphics g, Pen pen, Port portA, Port portB )
		{
			PaintConnection( g, pen, portA.Location, portB.Location );
		}

		public virtual void PaintConnection( Graphics g, Pen pen, Port portA, Point pointB )
		{
			PaintConnection( g, pen, portA.Location, pointB );
		}

		protected virtual void PaintConnection( Graphics g, Pen pen, Point pointA, Point pointB )
		{
			g.DrawLine( pen, pointA, pointB );
		}

		public Port GetPortByLocation( Point location )
		{
			foreach ( Port port in Ports )
			{
				if ( port.IntersectsWith( location ) )
				{
					return port;
				}
			}
			return null;
		}

		internal virtual void LoadCustomData( System.Xml.XmlElement customDataElement )
		{
		}

		internal virtual void SaveCustomData( System.Xml.XmlElement customDataElement )
		{
		}

		protected void AddAction( String actionName, NodeActionCreatorDelegate actionCreator )
		{
			m_actions.Add( actionName, actionCreator );
		}

		public NodeAction CreateAction( String actionName )
		{
			return m_actions[ actionName ]( this );
		}

		public NodeComponent getComponentByLocation( Point location )
		{
			foreach ( NodeComponent nc in m_components )
			{
				if ( nc.IntersectsWith( location ) )
				{
					return nc;
				}
			}
			return null;
		}
	}
}
