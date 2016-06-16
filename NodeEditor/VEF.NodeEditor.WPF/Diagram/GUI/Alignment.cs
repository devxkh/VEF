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

namespace Toothrot.Diagram.GUI
{
	public enum VerticalAlignment
	{
		TOP,
		CENTRE,
		BOTTOM
	}

	public enum HorizontalAlignment
	{
		LEFT,
		CENTRE,
		RIGHT
	}

	public class Aligner
	{
		public static int GetElementLeft( int parentLeft, int parentWidth, int elementWidth, HorizontalAlignment alignment )
		{
			if ( alignment == HorizontalAlignment.LEFT )
			{
				return parentLeft;
			}
			else if ( alignment == HorizontalAlignment.CENTRE )
			{
				return ( int )( parentLeft + parentWidth * 0.5 - elementWidth * 0.5 );
			}
			else
			{
				return parentLeft + parentWidth - elementWidth;
			}
		}

		public static int GetElementTop( int parentTop, int parentHeight, int elementHeight, VerticalAlignment alignment )
		{
			if ( alignment == VerticalAlignment.TOP )
			{
				return parentTop;
			}
			else if ( alignment == VerticalAlignment.CENTRE )
			{
				return ( int )( parentTop + parentHeight * 0.5 - elementHeight * 0.5 );
			}
			else
			{
				return parentTop + parentHeight - elementHeight;
			}
		}
	}
}
