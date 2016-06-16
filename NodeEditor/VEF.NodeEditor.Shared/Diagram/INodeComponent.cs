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

namespace Toothrot.Diagram
{
	public interface INodeComponent
	{
		int OffsetX
		{
			get;
			set;
		}

		int OffsetY
		{
			get;
			set;
		}

		Point Location
		{
			get;
			set;
		}

		Size Size
		{
			get;
			set;
		}

		int Width
		{
			get;
		}

		int Height
		{
			get;
		}

		Node Node
		{
			get;
			set;
		}

		IElementParent Parent
		{
			get;
			set;
		}

        //Padding Margins
        //{
        //    get;
        //    set;
        //}

		event EventHandler SizeChanged;
		event EventHandler LocationChanged;

		void UpdateLocation();

		void BringToFront();

		//void Register( DiagramControl diagramControl ); //Ugh!

		void Unregister();

		void Draw( Graphics g, Font f );
	}
}
