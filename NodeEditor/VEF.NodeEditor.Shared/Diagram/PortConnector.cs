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

namespace Toothrot.Diagram
{
	public class PortConnectedEventArgs : EventArgs
	{
		Port m_portFrom;
		Port m_portTo;

		public Port PortFrom
		{
			get { return m_portFrom; }
		}

		public Port PortTo
		{
			get { return m_portTo; }
		}

		public PortConnectedEventArgs( Port portFrom, Port portTo )
		{
			m_portFrom = portFrom;
			m_portTo = portTo;
		}
	}

	public class PortDisconnectedEventArgs : EventArgs
	{
		Port m_port;
		IEnumerable< Port > m_otherPorts;

		public Port Port
		{
			get { return m_port; }
		}

		public IEnumerable< Port > OtherPorts
		{
			get { return m_otherPorts; }
		}

		public PortDisconnectedEventArgs( Port port, IEnumerable< Port > otherPorts )
		{
			m_port = port;
			m_otherPorts = otherPorts;
		}
	}

	public class PortConnector
	{
		public event EventHandler< PortConnectedEventArgs > PortConnected;
		public event EventHandler< PortDisconnectedEventArgs > PortDisconnected;

		public PortConnector()
		{
		}

		public bool ValidConnection( Port portFrom, Port portTo )
		{
			if ( portFrom == portTo )
			{
				return false;
			}

			if ( portFrom == null )
			{
				return false;
			}

			if ( portTo == null )
			{
				return false;
			}

			return CheckConnectionValid( portFrom, portTo );
		}

		protected virtual bool CheckConnectionValid( Port portFrom, Port portTo )
		{
			return true;
		}

		public void Connect( Port portFrom, Port portTo )
		{
			
			if ( ! ValidConnection( portFrom, portTo ) )
			{
				return;
			}

			if ( portFrom.IsConnectedTo( portTo ) )
			{
				return;
			}

			if ( ! portFrom.AllowMultipleConnections )
			{
				Disconnect( portFrom );
			}

			if ( ! portTo.AllowMultipleConnections )
			{
				Disconnect( portTo );
			}

			portFrom.AddConnection( portTo );
			portTo.AddConnection( portFrom );

			if ( PortConnected != null )
			{
				PortConnected( this, new PortConnectedEventArgs( portFrom, portTo ) );
			}
		}

		public void Disconnect( Port port )
		{
			List< Port > disconnectedPorts = new List< Port >();
			foreach ( Port otherPort in port.Connections )
			{
				otherPort.RemoveConnection( port );
				disconnectedPorts.Add( otherPort );
			}

			port.ClearConnections();

			if ( PortDisconnected != null )
			{
				PortDisconnected( this, new PortDisconnectedEventArgs( port, disconnectedPorts ) );
			}
		}

		public void Disconnect( Port portFrom, Port portTo )
		{
			if ( ! portFrom.IsConnectedTo( portTo ) )
			{
				return;
			}

			portFrom.RemoveConnection( portTo );
			portTo.RemoveConnection( portFrom );
	
			if ( PortDisconnected != null )
			{
				List< Port > disconnectedPorts = new List< Port >();
				disconnectedPorts.Add( portTo );
				PortDisconnected( this, new PortDisconnectedEventArgs( portFrom, disconnectedPorts ) );
			}
		}
	}
}
