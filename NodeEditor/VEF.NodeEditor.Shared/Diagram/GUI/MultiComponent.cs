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

namespace Toothrot.Diagram.GUI
{
	public class MultiComponent : NodeComponent
	{

		public enum Orientation
		{
			HORIZONTAL,
			VERTICAL
		}

		List< INodeComponent > m_childComponents;
		int m_spacing;

		// TODO: be able to change orientation.
		Orientation m_orientation;

		// TODO: inner components should be able to be aligned


		public MultiComponent()
		{
			m_childComponents = new List< INodeComponent >();
			m_spacing = 4;

			this.LocationChanged += new EventHandler( MultiComponent_LocationChanged );
		}

		void MultiComponent_LocationChanged( object sender, EventArgs e )
		{
			UpdateChildLocations();
		}

		private void UpdateChildLocations()
		{
			foreach ( INodeComponent nc in m_childComponents )
			{
				nc.Location = new Point( Left + nc.OffsetX, Top + nc.OffsetY );
			}
		}

		public void AddComponent( INodeComponent component )
		{
			component.SizeChanged += new EventHandler( component_SizeChanged );

			m_childComponents.Add( component );

			RecalculateSize();
		}

		void component_SizeChanged( object sender, EventArgs e )
		{
			RecalculateSize();
		}

		private void RecalculateSize()
		{
			// Currently ignores child component margins...

			int width = 0;
			int height = 0;
			
			foreach ( INodeComponent nc in m_childComponents )
			{
				if ( m_orientation == Orientation.HORIZONTAL )
				{
					nc.OffsetX = width;
					nc.OffsetY = 0;
					width += nc.Width + m_spacing;
					height = Math.Max( height, nc.Height );
				}
				else
				{
					nc.OffsetX = 0;
					nc.OffsetY = height;
					width = Math.Max( width, nc.Width );
					height += nc.Height + m_spacing;
				}
			}
			
			foreach ( INodeComponent nc in m_childComponents )
			{
				if ( m_orientation == Orientation.HORIZONTAL )
				{
					nc.Size = new Size( nc.Width, height );
					
				}
				else
				{
					nc.Size = new Size( width, nc.Height );
				}
			}
			

			Size = new Size( width, height );
		}

		public override void BringToFront()
		{
			foreach ( INodeComponent nc in m_childComponents )
			{
				nc.BringToFront();
			}
		}

        //public override void Register( DiagramControl diagramControl )
        //{
        //    foreach ( INodeComponent nc in m_childComponents )
        //    {
        //        nc.Register( diagramControl );
        //    }
        //}

		public override void Unregister()
		{
			foreach ( INodeComponent nc in m_childComponents )
			{
				nc.Unregister();
			}
		}

		public override void Draw( Graphics g, Font f )
		{
			foreach ( INodeComponent nc in m_childComponents )
			{
//				g.DrawRectangle( Pens.SpringGreen, new Rectangle( nc.Location, nc.Size ) );
				nc.Draw( g, f );
			}
		}

	}
}
