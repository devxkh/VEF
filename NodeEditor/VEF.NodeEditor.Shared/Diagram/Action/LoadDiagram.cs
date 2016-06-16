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
using System.Reflection;
using System.Runtime.Remoting;
using System.Drawing;

namespace Toothrot.Diagram.Action
{
	public class LoadDiagram : DiagramAction
	{
		String m_filename;
		Dictionary< int, Node > m_internalNodeIds;
		

		public LoadDiagram( Diagram diagram, String filename )
			: base( "Load Diagram", diagram )
		{
			m_filename = filename;

			AddHistoryOperation( HistoryOperation.CLEAR_ON_SUCCESS );
		}

		protected override ActionResult OnExecute()
		{
			// Clear Diagram!
			Diagram.RemoveAllNodes();

			m_internalNodeIds = new Dictionary< int, Node >();
			
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load( m_filename );

			XmlElement rootElement = xmlDocument.DocumentElement;

            //int version = Helper.Xml.ReadInt( rootElement.Attributes[ "version" ] );

            //if ( version != 1 )
            //{
            //    FailureReason = "Can't load this version of diagram files";
            //    return ActionResult.FAILURE;
            //}

            //XmlElement nodeListElement = Helper.Xml.GetFirstChildElement( rootElement, "node_list" );
            //if ( nodeListElement == null )
            //{
            //    FailureReason = "Node list not found";
            //    return ActionResult.FAILURE;
            //}

            //foreach ( XmlNode xmlNode in nodeListElement.ChildNodes )
            //{
            //    if ( xmlNode.NodeType == XmlNodeType.Element )
            //    {
            //        if ( xmlNode.Name == "node" )
            //        {
            //            int nodeId = Helper.Xml.ReadInt( xmlNode.Attributes[ "id" ] );
            //            String name = Helper.Xml.ReadString( xmlNode.Attributes[ "name" ] );
            //            String type = Helper.Xml.ReadString( xmlNode.Attributes[ "type" ] );

            //            Assembly assembly = Assembly.GetEntryAssembly();
            //            ObjectHandle nodeHandle = Activator.CreateInstance( assembly.GetName().FullName, type );
            //            Node node = ( Node ) nodeHandle.Unwrap();
            //            node.Name = name;

            //            XmlElement locationElement = Helper.Xml.GetFirstChildElement( xmlNode, "location" );
            //            Point location = Helper.Xml.ReadPoint( locationElement );
            //            node.SetLocation( location );

            //            m_internalNodeIds[ nodeId ] = node;

            //            XmlElement customDataElement = Helper.Xml.GetFirstChildElement( xmlNode, "custom_data" );

            //            node.LoadCustomData( customDataElement );

            //            Diagram.AddNode( node );
            //        }
            //    }
            //}

            //XmlElement connectionListElement = Helper.Xml.GetFirstChildElement( rootElement, "connection_list" );
            //if ( connectionListElement == null )
            //{
            //    FailureReason = "Connection list not found";
            //    return ActionResult.FAILURE;
            //}

            //foreach ( XmlNode connectionNode in connectionListElement.ChildNodes )
            //{
            //    if ( connectionNode.NodeType == XmlNodeType.Element )
            //    {
            //        if ( connectionNode.Name == "connection" )
            //        {
            //            int nodeAId = Helper.Xml.ReadInt( connectionNode.Attributes[ "node_a" ] );
            //            String portAName = Helper.Xml.ReadString( connectionNode.Attributes[ "port_a" ] );
            //            int nodeBId = Helper.Xml.ReadInt( connectionNode.Attributes[ "node_b" ] );
            //            String portBName = Helper.Xml.ReadString( connectionNode.Attributes[ "port_b" ] );

            //            Node nodeA = m_internalNodeIds[ nodeAId ];
            //            Port portA = nodeA.GetPortByName( portAName );
            //            Node nodeB = m_internalNodeIds[ nodeBId ];
            //            Port portB = nodeB.GetPortByName( portBName );

            //            portA.AddConnection( portB );
            //        }
            //    }
            //}

			return ActionResult.SUCCESS;
		}
	}
}
