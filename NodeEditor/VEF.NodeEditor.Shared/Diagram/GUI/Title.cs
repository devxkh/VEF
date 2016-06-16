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
using System.Drawing.Drawing2D;

namespace Toothrot.Diagram.GUI
{
	// should be the first component added to a node to work correctly.
	public class Title : Label
	{
		float m_roundness = 5;

		Color m_colour1 = Color.Black;
		Color m_colour2 = Color.DarkGray;

		public float Roundness
		{
			get { return m_roundness; }
			set { m_roundness = value; }
		}

		public Color Colour1
		{
			get { return m_colour1; }
			set { m_colour1 = value; }
		}
		

		public Color Colour2
		{
			get { return m_colour2; }
			set { m_colour2 = value; }
		}

		public Title()
		{
			HorizontalTextAlignment = HorizontalAlignment.CENTRE;
			VerticalTextAlignment = VerticalAlignment.CENTRE;
			TextColour = Color.White;
		//	Margins = new Padding( Margins.Left, Margins.Top, Margins.Right, 6 );
		}

		public override void Draw( Graphics g, Font f )
		{
		//	Rectangle rectangle = new Rectangle( Left - Margins.Left, Top - Margins.Top, Width + Margins.Horizontal, Height + Margins.Vertical );
            Rectangle rectangle = new Rectangle(Left - 5, Top - 5, Width + 5, Height + 5);

			Drawing.FillUpperHalfRoundRectangle( g, new LinearGradientBrush( rectangle, Colour1, Colour2, LinearGradientMode.Vertical ), rectangle, Roundness );
			
			base.Draw( g, f );
		}
	}

	
}
