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
using System.Xml;
using Toothrot.Action;

namespace Toothrot.Diagram.Action
{
	public class SaveDiagram : DiagramAction
	{
		String m_filename;
		Dictionary< Node, int > m_internalNodeIds;

		public SaveDiagram( Diagram diagram, string filename )
			: base( "Save Diagram", diagram )
		{
			m_filename = filename;

			// TODO: clear changes flag on success.
		}

		protected override ActionResult OnExecute()
		{
			m_internalNodeIds = new Dictionary< Node, int >();

			XmlDocument xmlDocument = new XmlDocument();

            //XmlElement rootElement = Helper.Xml.CreateElement( xmlDocument, "diagram" );
            //Helper.Xml.CreateAttribute( rootElement, "version", 1 );

            //// save diagram size

            //// save diagram custom properties

            //XmlElement nodeListElement = Helper.Xml.CreateElement( rootElement, "node_list" );
            //foreach ( Node node in Diagram.Nodes )
            //{
            //    int nodeId = m_internalNodeIds.Count;
            //    m_internalNodeIds[ node ] = nodeId;

            //    XmlElement nodeElement = Helper.Xml.CreateElement( nodeListElement, "node" );
            //    Helper.Xml.CreateAttribute( nodeElement, "id", nodeId );
            //    Helper.Xml.CreateAttribute( nodeElement, "name", node.Name );
            //    Helper.Xml.CreateAttribute( nodeElement, "type", node.GetType().ToString() );

            //    Helper.Xml.CreateElement( nodeElement, "location", node.Location );

            //    XmlElement customDataElement = Helper.Xml.CreateElement( nodeElement, "custom_data" );
            //    node.SaveCustomData( customDataElement );
            //}

            //XmlElement connectionListElement = Helper.Xml.CreateElement( rootElement, "connection_list" );

            //foreach ( Node nodeA in Diagram.Nodes )
            //{
            //    int nodeAID = m_internalNodeIds[ nodeA ]; 
            //    foreach ( Port portA in nodeA.Ports )
            //    {
            //        foreach ( Port portB in portA.Connections )
            //        {
            //            Node nodeB = portB.Node;
            //            int nodeBID = m_internalNodeIds[ nodeB ];

            //            XmlElement connectionElement = Helper.Xml.CreateElement( connectionListElement, "connection" );
            //            Helper.Xml.CreateAttribute( connectionElement, "node_a", nodeAID );
            //            Helper.Xml.CreateAttribute( connectionElement, "port_a", portA.Name );
            //            Helper.Xml.CreateAttribute( connectionElement, "node_b", nodeBID );
            //            Helper.Xml.CreateAttribute( connectionElement, "port_b", portB.Name );
            //        }
            //    }
            //}

            //xmlDocument.Save( m_filename );

			return ActionResult.SUCCESS;
		}
	}
}
