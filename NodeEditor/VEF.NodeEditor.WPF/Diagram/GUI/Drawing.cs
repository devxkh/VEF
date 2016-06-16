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
	class Drawing
	{
		public static GraphicsPath GetRoundRectanglePath( Rectangle rectangle, float radius )
		{
			if ( radius <= 0 )
			{
				GraphicsPath rectanglePath = new GraphicsPath();
				rectanglePath.AddRectangle( rectangle );
				return rectanglePath;
			}

			float size = radius * 2f;

			GraphicsPath roundRectanglePath = new GraphicsPath();
			roundRectanglePath.AddArc( rectangle.X, rectangle.Y, size, size, 180, 90 );
			roundRectanglePath.AddArc( rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90 );
			roundRectanglePath.AddArc( rectangle.X + rectangle.Width - size, rectangle.Y + rectangle.Height - size, size, size, 0, 90 );
			roundRectanglePath.AddArc( rectangle.X, rectangle.Y + rectangle.Height - size, size, size, 90, 90 );
			roundRectanglePath.CloseFigure();

			return roundRectanglePath;
		}

		public static GraphicsPath GetUpperHalfRoundRectanglePath( Rectangle rectangle, float radius )
		{
			if ( radius <= 0 )
			{
				GraphicsPath rectanglePath = new GraphicsPath();
				rectanglePath.AddRectangle( rectangle );
				return rectanglePath;
			}

			float size = radius * 2f;

			GraphicsPath roundRectanglePath = new GraphicsPath();
			roundRectanglePath.AddArc( rectangle.X, rectangle.Y, size, size, 180, 90 );
			roundRectanglePath.AddArc( rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90 );
			roundRectanglePath.AddLine( rectangle.Right, rectangle.Bottom, rectangle.Left, rectangle.Bottom );
			roundRectanglePath.CloseFigure();

			return roundRectanglePath;
		}

		public static void FillRoundRectangle( Graphics g, Brush brush, Rectangle rectangle, float radius )
		{
			if ( radius <= 0 )
			{
				g.FillRectangle( brush, rectangle );
				return;
			}

			GraphicsPath path = GetRoundRectanglePath( rectangle, radius );
			g.FillPath( brush, path );
			path.Dispose();
		}

		public static void DrawRoundRectangle( Graphics g, Pen pen, Rectangle rectangle, float radius )
		{
			if ( radius <= 0 )
			{
				g.DrawRectangle( pen, rectangle );
				return;
			}

			GraphicsPath path = GetRoundRectanglePath( rectangle, radius );
			g.DrawPath( pen, path );
			path.Dispose();
		}

		public static void FillShadowRoundRectangle( Graphics g, Color shadowColor, float totalOpacity, int iterations, Rectangle rectangle, float radius )
		{
			int opacity = ( int )( ( totalOpacity * 255 ) / iterations );
			Brush shadowBrush = new SolidBrush( Color.FromArgb( opacity, shadowColor ) );

			Rectangle shadowRectangle = rectangle;
			for ( int i = 0; i < iterations; i++ )
			{
				shadowRectangle.Offset( 1, 1 );
				FillRoundRectangle( g, shadowBrush, shadowRectangle, radius );
			}

			shadowBrush.Dispose();
		}

		public static void FillUpperHalfRoundRectangle( Graphics g, Brush brush, Rectangle rectangle, float radius )
		{
			if ( radius <= 0 )
			{
				g.FillRectangle( brush, rectangle );
				return;
			}

			GraphicsPath path = GetUpperHalfRoundRectanglePath( rectangle, radius );
			g.FillPath( brush, path );
			path.Dispose();
		}

	}
}
