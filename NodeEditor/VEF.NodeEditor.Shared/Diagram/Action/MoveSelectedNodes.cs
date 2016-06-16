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
using Toothrot.Action;

namespace Toothrot.Diagram.Action
{
	public class MoveSelectedNodes : DiagramAction
	{

		int m_totalMovementX;
		int m_totalMovementY;

		Point m_selectionOffset;

		int m_minX;
		int m_minY;
		int m_maxX;
		int m_maxY;

// 		Point m_nextMovement;

		public MoveSelectedNodes( Diagram diagram, Point movementStartLocation, Size canvasSize )
			: base( "Move Selected Nodes", diagram )
		{
			m_totalMovementX = 0;
			m_totalMovementY = 0;

			int selectionOffsetX = movementStartLocation.X - Diagram.SelectionExtents.X;
			int selectionOffsetY = movementStartLocation.Y - Diagram.SelectionExtents.Y;

			m_selectionOffset = new Point( selectionOffsetX, selectionOffsetY );

			m_minX = 0;
			m_minY = 0;
			m_maxX = canvasSize.Width - Diagram.SelectionExtents.Width;
			m_maxY = canvasSize.Height - Diagram.SelectionExtents.Height;

			AddHistoryOperation( HistoryOperation.STORE_ON_SUCCESS );
		}

		public void MoveNodesToLocation( Point location )
		{
			int desiredX = location.X - m_selectionOffset.X;
			int desiredY = location.Y - m_selectionOffset.Y;

			int destinationX = Math.Max( m_minX, Math.Min( desiredX, m_maxX ) );
			int destinationY = Math.Max( m_minY, Math.Min( desiredY, m_maxY ) );

			int dx = destinationX - Diagram.SelectionExtents.X;
			int dy = destinationY - Diagram.SelectionExtents.Y;

			m_totalMovementX += dx;
			m_totalMovementY += dy;

			Diagram.MoveSelectedNodes( dx, dy );
		}

		override protected ActionResult OnExecute()
		{
			if ( m_totalMovementX == 0 && m_totalMovementY == 0 )
			{
				FailureReason = "No movement";
				return ActionResult.FAILURE;
			}

			return ActionResult.SUCCESS;
		}

		override protected ActionResult OnUndo()
		{
			Diagram.MoveSelectedNodes( -m_totalMovementX, -m_totalMovementY );
			return ActionResult.SUCCESS;
		}

		override protected ActionResult OnRedo()
		{
			Diagram.MoveSelectedNodes( m_totalMovementX, m_totalMovementY );
			return ActionResult.SUCCESS;
		}

	}
}
