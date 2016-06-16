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

namespace Toothrot.Diagram.Example
{
	public enum SimplePortType
	{
		IN,
		OUT
	}

	public class SimplePort : Port
	{
		SimplePortType m_portType;

		public SimplePortType PortType
		{
			get { return m_portType; }
			set
			{ 
				m_portType = value;
				UpdatePortPropertiesBasedOnType();
			}
		}


		public SimplePort( String name, SimpleNode node )
			: base( name, node )
		{
			RelativePositionIsPixels = true;
			VerticalPortAlign = VPortAlign.CENTRE;
			RelativePositionToParent = new PointF( -8, 0 );

			UpdatePortPropertiesBasedOnType();
		}

		public Point GetBezierControlPoint()
		{
			if ( PortType == SimplePortType.IN )
			{
				return Point.Add( Location, new Size( -40, 0 ) );
			}
			else
			{
				return Point.Add( Location, new Size( 40, 0 ) );
			}
		}

		protected virtual void UpdatePortPropertiesBasedOnType()
		{
			if ( PortType == SimplePortType.OUT )
			{				
				HorizontalPortAlign = HPortAlign.RIGHT;
			}
			else
			{
				HorizontalPortAlign = HPortAlign.LEFT;
			}
		}


		public override void Paint( Graphics g, Font f )
		{
            //Image portImage = Toothrot.Diagram.GUI.PortImages.RIGHT;
            //g.DrawImage(portImage, Dimensions);
        }
	}
}
