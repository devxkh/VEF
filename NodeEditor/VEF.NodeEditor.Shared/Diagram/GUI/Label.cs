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

using Toothrot.Diagram;
using System.Drawing;

namespace Toothrot.Diagram.GUI
{
	public class Label : NodeComponent
	{
		String m_text;
		Font m_font;

		Size m_textSize;
		VerticalAlignment m_verticalTextAlignment;
		HorizontalAlignment m_horizontalTextAlignment;

		Color m_textColour;
		Brush m_textBrush;

		public Color TextColour
		{
			get { return m_textColour; }
			set
			{ 
				m_textColour = value;
				m_textBrush = new SolidBrush( TextColour );
			}
		}

		public event EventHandler TextChanged;

		public String Text
		{
			get { return m_text; }
			set
			{ 
				if ( m_text == value )
				{
					return;
				}

				m_text = value;
			//	m_textSize = TextRenderer.MeasureText( Text, m_font );
				Size = m_textSize;

				if ( TextChanged != null )
				{
					TextChanged( this, null );
				}
			}
		}

		public VerticalAlignment VerticalTextAlignment
		{
			get { return m_verticalTextAlignment; }
			set { m_verticalTextAlignment = value; }
		}

		public HorizontalAlignment HorizontalTextAlignment
		{
			get { return m_horizontalTextAlignment; }
			set { m_horizontalTextAlignment = value; }
		}

		public Label()
		{
			m_text = "";
		//	m_font = System.Windows.Forms.Control.DefaultFont;
			TextColour = Color.Black;
		}

		public override void Draw( Graphics g, Font f )
		{
			int textLeft = CalculateTextLeft();
			int textTop = CalculateTextTop();
			g.DrawString( Text, m_font, m_textBrush, textLeft, textTop );
		}

		protected int CalculateTextLeft()
		{
			return Aligner.GetElementLeft( Left, Width, m_textSize.Width, HorizontalTextAlignment );
		}

		protected int CalculateTextTop()
		{
			return Aligner.GetElementTop( Top, Height, m_textSize.Height, VerticalTextAlignment );
		}
	}
}
