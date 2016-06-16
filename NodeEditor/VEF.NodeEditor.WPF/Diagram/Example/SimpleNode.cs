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
using Toothrot.Diagram.GUI;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Toothrot.Diagram.Example
{
	public class SimpleNode : Node
	{
		Title m_titleComponent;

		Color m_bodyColour1 = Color.LightGray;
		Color m_bodyColour2 = Color.White;

		Color m_bodyTextColour = Color.Black;
		
		float m_roundness = 5;

		public float Roundness
		{
			get { return m_roundness; }
			set
			{ 
				m_roundness = value;
				m_titleComponent.Roundness = value;
			}
		}

		public Color BodyColour1
		{
			get { return m_bodyColour1; }
			set { m_bodyColour1 = value; }
		}
		
		public Color BodyColour2
		{
			get { return m_bodyColour2; }
			set { m_bodyColour2 = value; }
		}

		public Color BodyTextColour
		{
			get { return m_bodyTextColour; }
			set { m_bodyTextColour = value; }
		}

		public Color TitleColour1
		{
			get { return m_titleComponent.Colour1; }
			set { m_titleComponent.Colour1 = value; }
		}

		public Color TitleColour2
		{
			get { return m_titleComponent.Colour2; }
			set { m_titleComponent.Colour2 = value; }
		}

		public Color TitleTextColour
		{
			get { return m_titleComponent.TextColour; }
			set { m_titleComponent.TextColour = value; }
		}

		public SimpleNode()
		{
			CreateTitle();
		}

		private void CreateTitle()
		{
			m_titleComponent = new Title();
			m_titleComponent.Text = Name;
			NameChanged += new EventHandler( SimpleNode_NameChanged );
			AddComponent( m_titleComponent );

			Separator separator = new Separator( SeparatorDirection.HORIZONTAL );
			AddComponent( separator );
		}

		void SimpleNode_NameChanged( object sender, EventArgs e )
		{
			m_titleComponent.Text = Name;
		}

		public override void Paint( Graphics g, Font f )
		{
			GUI.Drawing.FillShadowRoundRectangle( g, Color.Black, 0.7f, 3, Dimensions, Roundness );

			Brush bodyBrush = new LinearGradientBrush( Dimensions, BodyColour1, BodyColour2, LinearGradientMode.Vertical );

			GUI.Drawing.FillRoundRectangle( g, bodyBrush, Dimensions, Roundness );

			bodyBrush.Dispose();

			foreach ( NodeComponent nc in NodeComponents )
			{
				nc.Draw( g, f );
			}

			if ( Selected )
			{
				GUI.Drawing.DrawRoundRectangle( g, new Pen( Color.Black, 3 ), Dimensions, Roundness );
				GUI.Drawing.DrawRoundRectangle( g, new Pen( Color.White, 2 ), Dimensions, Roundness );
			}
			else
			{
				GUI.Drawing.DrawRoundRectangle( g, new Pen( Color.Black, 2 ), Dimensions, Roundness );
			}

			foreach ( Port port in Ports )
			{
				port.Paint( g, f );
			}
		}

		public override void PaintOutConnections( Graphics g, Pen pen )
		{
			foreach ( Port port in Ports )
			{
				SimplePort basePort = ( SimplePort ) port;
				if ( basePort.PortType == SimplePortType.OUT )
				{
					foreach ( Port otherPort in port.Connections )
					{
						PaintConnection( g, pen, port, otherPort );
					}
				}
			}
		}

		public override void PaintConnection( Graphics g, Pen pen, Port portA, Port portB )
		{
			SimplePort simplePortA = ( SimplePort ) portA;
			SimplePort simplePortB = ( SimplePort ) portB;
			PaintConnection( g, pen, portA.Location, simplePortA.GetBezierControlPoint(), simplePortB.GetBezierControlPoint(), portB.Location );
		}

		public override void PaintConnection( Graphics g, Pen pen, Port portA, Point pointB )
		{
			SimplePort simplePortA = ( SimplePort ) portA;

			PaintConnection( g, pen, portA.Location, simplePortA.GetBezierControlPoint(), pointB, pointB );

		}

		protected override void PaintConnection( Graphics g, Pen pen, Point pointA, Point pointB )
		{
			Point p2 = pointA;
			p2.Offset( 40, 0 );

			Point p3 = pointB;
			p3.Offset( -40, 0 );

			PaintConnection( g, pen, pointA, p2, p3, pointB );
		}

		protected void PaintConnection( Graphics g, Pen pen, Point p1, Point p2, Point p3, Point p4 )
		{
			DrawConnectionShadow( g, 2, 0.3f, p1, p2, p3, p4 );
			g.DrawBezier( pen, p1, p2, p3, p4 );
		}

		private void DrawConnectionShadow( Graphics g, int offset, float opacity, Point p1, Point p2, Point p3, Point p4 )
		{
			p1.Offset( offset, offset );
			p2.Offset( offset, offset );
			p3.Offset( offset, offset );
			p4.Offset( offset, offset );

			Pen shadowPen = new Pen( Color.FromArgb( ( int ) ( opacity * 255 ), Color.Black ), 2 );
			g.DrawBezier( shadowPen, p1, p2, p3, p4 );
			shadowPen.Dispose();
		}
	}
}
