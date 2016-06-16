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

namespace Toothrot.Diagram.Example
{
	public class SimplePortConnector : PortConnector
	{

		protected override bool CheckConnectionValid( Port portFrom, Port portTo )
		{
			SimplePort simplePortFrom = ( SimplePort ) portFrom;
			SimplePort simplePortTo = ( SimplePort ) portTo;

			if ( simplePortFrom.PortType == simplePortTo.PortType )
			{
				return false;
			}

			bool loopWouldBeCreatedIfConnected = CheckLoop( simplePortFrom, simplePortTo );
			if ( loopWouldBeCreatedIfConnected )
			{
				return false;
			}

			return true;
		}

		protected bool CheckLoop( SimplePort portA, SimplePort portB )
		{
			if ( portA.PortType == SimplePortType.OUT )
			{
				return CheckLoopRec( portB, ( SimpleNode ) portA.Node );
			}

			if ( portB.PortType == SimplePortType.OUT )
			{
				return CheckLoopRec( portA, ( SimpleNode ) portB.Node );
			}

			return false;
		}

		bool CheckLoopRec( SimplePort currentPort, SimpleNode startNode )
		{
			SimpleNode currentNode = ( SimpleNode ) currentPort.Node;
			
			if ( currentNode == startNode )
			{
				return true;
			}


			foreach ( SimplePort port in currentNode.Ports )
			{
				if ( port.PortType == SimplePortType.OUT )
				{
					if ( ShouldCheckLoopThroughLeavingPort( currentPort, port ) )
					{
						foreach ( SimplePort connectedPort in port.Connections )
						{
							bool loopFound = CheckLoopRec( connectedPort, startNode );
							if ( loopFound )
							{
								return true;
							}
						}
					}	
				}
			}

			return false;

		}

		// Additional checks to decide to see if a port should be inspected when looking for loops.
		protected virtual bool ShouldCheckLoopThroughLeavingPort( SimplePort portArrivedToNode, SimplePort portLeavingNode )
		{
			return true;
		}
	}
}
