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
using Toothrot.Action;

namespace Toothrot.Diagram.Action
{
	public class ConnectPorts : DiagramAction
	{
		PortConnector m_connector;
		Port m_portFrom;
		Port m_portTo;

		List< PortDisconnectedEventArgs > m_connectionsToRestore;

		public ConnectPorts( Diagram diagram, PortConnector connector, Port portFrom, Port portTo )
			: base( "Connect Ports", diagram )
		{
			m_connector = connector;
			m_portFrom = portFrom;
			m_portTo = portTo;
			m_connectionsToRestore = new List< PortDisconnectedEventArgs >();

			AddHistoryOperation( HistoryOperation.STORE_ON_SUCCESS );
		}

		protected override ActionResult OnExecute()
		{
			m_connector.PortDisconnected += new EventHandler< PortDisconnectedEventArgs >( PortDisconnected );
			m_connector.Connect( m_portFrom, m_portTo );
			m_connector.PortDisconnected -= new EventHandler< PortDisconnectedEventArgs >( PortDisconnected );

			return ActionResult.SUCCESS;
		}

		void PortDisconnected( object sender, PortDisconnectedEventArgs e )
		{
			m_connectionsToRestore.Add( e );
		}

		override protected ActionResult OnUndo()
		{
			m_connector.Disconnect( m_portFrom, m_portTo );

			foreach ( PortDisconnectedEventArgs e in m_connectionsToRestore )
			{
				foreach ( Port portTo in e.OtherPorts )
				{
					m_connector.Connect( e.Port, portTo );
				}
			}

			return ActionResult.SUCCESS;
		}

		override protected ActionResult OnRedo()
		{
			m_connector.Connect( m_portFrom, m_portTo );

			return ActionResult.SUCCESS;
		}

	}
}
