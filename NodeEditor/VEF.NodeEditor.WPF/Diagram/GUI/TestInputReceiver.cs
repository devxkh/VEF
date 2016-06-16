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

namespace Toothrot.Diagram.GUI
{
	public class TestInputReceiver : NodeComponent
	{
		float m_x;
		float m_y;

		bool m_mouseIn;

		public TestInputReceiver()
		{
			m_mouseIn = false;
			Size = new Size( 128, 128 );

			m_x = 0.5f;
			m_y = 0.5f;
		}

		public override void Draw( Graphics g, Font f )
		{
			int x = Left + ( int )( m_x * Width );
			int y = Top + ( int )( m_y * Height );
			if ( m_mouseIn )
			{
				g.FillRectangle( Brushes.LightGray, new Rectangle( Location, Size ) );
			}
			else
			{
				g.FillRectangle( Brushes.DarkGray, new Rectangle( Location, Size ) );
			}
			
			g.DrawRectangle( Pens.Black, new Rectangle( Location, Size ) );
			
			int s = 3;
			g.FillEllipse( Brushes.DarkRed, x - s, y - s, 2 * s, 2 * s );
		}

		public override bool MouseDownEvent( MouseEventArgs e )
		{
			UpdatePointLocation( e );

			return true;
		}

		public override bool MouseMoveEvent( MouseEventArgs e )
		{
			if ( e.LeftButton != MouseButtonState.Pressed)
			{
				return false;
			}

			UpdatePointLocation( e );

			return true;
		}

		private void UpdatePointLocation( MouseEventArgs e )
		{
            //todo?
            //Point location = ConvertPointToLocalCoordinates( e.Location );
            //m_x = ( ( float ) ( location.X ) ) / Width;
            //m_y = ( ( float ) ( location.Y ) ) / Height;

            //m_x = Math.Max( 0, Math.Min( m_x, 1 ) );
            //m_y = Math.Max( 0, Math.Min( m_y, 1 ) );
		}

		public override bool MouseExitEvent( MouseEventArgs e )
		{
			m_mouseIn = false;

			if ( e.LeftButton == MouseButtonState.Pressed)
			{
				UpdatePointLocation( e );
			}
			

			return true;
		}

		public override bool MouseEnterEvent( MouseEventArgs e )
		{
			m_mouseIn = true;

			return true;
		}
	}
}
