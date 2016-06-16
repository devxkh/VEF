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
	public enum SeparatorDirection
	{
		HORIZONTAL,
		VERTICAL
	}

	public class Separator : NodeComponent
	{
		SeparatorDirection m_direction;
		float m_lineWidth;
		Color m_lineColor;

		public float LineWidth
		{
			get { return m_lineWidth; }
			set { m_lineWidth = value; }
		}

		public Color LineColor
		{
			get { return m_lineColor; }
			set { m_lineColor = value; }
		}

		public Separator( SeparatorDirection direction )
		{
			Size = new Size( 1, 1 );
			m_direction = direction;
		//	Margins = new Padding( 0 );
			LineWidth = 1;
			LineColor = Color.Black;
		}

		public override void Draw( Graphics g, Font f )
		{
			Pen pen = new Pen( LineColor, LineWidth );
			if ( m_direction == SeparatorDirection.HORIZONTAL )
			{
				g.DrawLine( pen, Left, Top, Left + Width, Top );
			}
			else
			{
				g.DrawLine( pen, Left, Top, Left, Top + Height );
			}
			pen.Dispose();
		}
	}
}
