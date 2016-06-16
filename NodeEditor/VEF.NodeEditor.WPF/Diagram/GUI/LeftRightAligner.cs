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
	public class LeftRightAligner : NodeComponent
	{
		INodeComponent m_leftComponent = null;
		INodeComponent m_rightComponent = null;

		public INodeComponent LeftComponent
		{
			get { return m_leftComponent; }
		}

		public INodeComponent RightComponent
		{
			get { return m_rightComponent; }
		}

		public LeftRightAligner( INodeComponent leftComponent, INodeComponent rightComponent )
		{
			if ( leftComponent != null )
			{
				m_leftComponent = leftComponent;
				m_leftComponent.Parent = this;
				m_leftComponent.SizeChanged += new EventHandler( component_SizeChanged );
			}

			if ( rightComponent != null )
			{
				m_rightComponent = rightComponent;
				m_rightComponent.Parent = this;
				m_rightComponent.SizeChanged += new EventHandler( component_SizeChanged );
			}

			LocationChanged += new EventHandler( LeftRightAligner_LocationChanged );
			SizeChanged += new EventHandler( LeftRightAligner_SizeChanged );

			RecalculateSize();
		}

		void LeftRightAligner_SizeChanged( object sender, EventArgs e )
		{
			UpdateChildOffsets();
			UpdateChildLocations();
		}

		void LeftRightAligner_LocationChanged( object sender, EventArgs e )
		{
			UpdateChildLocations();
		}

		void UpdateChildLocations()
		{
			UpdateChildLocation( LeftComponent );
			UpdateChildLocation( RightComponent );
		}

		void UpdateChildLocation( INodeComponent childComponent )
		{
			if ( childComponent != null )
			{
				childComponent.Location = new Point( Left + childComponent.OffsetX, Top + childComponent.OffsetY );
			}
		}

		void component_SizeChanged( object sender, EventArgs e )
		{
			RecalculateSize();
		}


		void RecalculateSize()
		{
			int minSeparation = 4;

			int leftWidth = 0;
			int leftHeight = 0;
			int rightWidth = 0;
			int rightHeight = 0;

			if ( LeftComponent != null )
			{
				leftWidth = LeftComponent.Width;
				leftHeight = LeftComponent.Height;
			}

			if ( RightComponent != null )
			{
				rightWidth = RightComponent.Width;
				rightHeight = RightComponent.Height;
			}

			int width = leftWidth + rightWidth + minSeparation;
			int height = Math.Max( leftHeight, rightHeight );

			Size = new Size( width, height );
		}

		void UpdateChildOffsets()
		{
			if ( LeftComponent != null )
			{
				LeftComponent.OffsetX = 0;
				LeftComponent.OffsetY = ( Height - LeftComponent.Height ) / 2;
			}
			
			if ( RightComponent != null )
			{
				RightComponent.OffsetX = Width - RightComponent.Width;
				RightComponent.OffsetY = ( Height - RightComponent.Height ) / 2;
			}
		}

		public override void BringToFront()
		{
			if ( LeftComponent != null )
			{
				LeftComponent.BringToFront();
			}

			if ( RightComponent != null )
			{
				RightComponent.BringToFront();
			}
		}

        //public override void Register( DiagramControl diagramControl )
        //{
        //    if ( LeftComponent != null )
        //    {
        //        LeftComponent.Register( diagramControl );
        //    }

        //    if ( RightComponent != null )
        //    {
        //        RightComponent.Register( diagramControl );
        //    }
        //}

		public override void Unregister()
		{
			if ( LeftComponent != null )
			{
				LeftComponent.Unregister();
			}

			if ( RightComponent != null )
			{
				RightComponent.Unregister();
			}
		}

		public override void Draw( Graphics g, Font f )
		{
			if ( LeftComponent != null )
			{
				LeftComponent.Draw( g, f );
			}

			if ( RightComponent != null )
			{
				RightComponent.Draw( g, f );
			}

			base.Draw( g, f );
		}
	}
}
