using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace LDPServer.Business
{
    public class RdfDirectory
    {
        public string GetDirectory(string path)
        {
            // Turtle is syntax used to describe the directory list
            var turtleWriter = new CompressingTurtleWriter();

            var g = new Graph();
            g.BaseUri = new Uri("https://localhost:44340/");
            // Clear RDF namespaces
            g.NamespaceMap.Clear();
            // Add LDP namespaces
            g.NamespaceMap.AddNamespace("n0", new Uri("https://localhost:44340/"));
            g.NamespaceMap.AddNamespace("tes", new Uri("https://localhost:44340/testfolder"));
            g.NamespaceMap.AddNamespace("ldp", new Uri("http://www.w3.org/ns/ldp#"));
            g.NamespaceMap.AddNamespace("terms", new Uri("http://purl.org/dc/terms/"));
            g.NamespaceMap.AddNamespace("XML", new Uri("http://www.w3.org/2001/XMLSchema#"));
            g.NamespaceMap.AddNamespace("st", new Uri("http://www.w3.org/ns/posix/stat#"));
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            // Nodes to be used in the connections (triple)
            var basicContainer = g.CreateUriNode("ldp:BasicContainer");
            var container = g.CreateUriNode("ldp:Container");
            var rescoure = g.CreateUriNode("ldp:Resource");
            var contains = g.CreateUriNode("ldp:contains");
            var rdfType = g.CreateUriNode("rdf:type");            
            var modified = g.CreateUriNode("terms:modified");
            var mtime = g.CreateUriNode("st:mtime");
            var size = g.CreateUriNode("st:size");
            // Root of graph
            var n0 = g.CreateUriNode();
            // Set types
            g.Assert(new Triple(n0, rdfType, basicContainer));
            g.Assert(new Triple(n0, rdfType, container));
            // Set values
            //g.Assert(new Triple(n0, modified, g.CreateLiteralNode("2018-11-20T12:25:08Z", "XML:dateTime"))); // breaks generation of turtle file
            g.Assert(new Triple(n0, mtime, g.CreateLiteralNode("1542716708", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInt))));
            g.Assert(new Triple(n0, size, g.CreateLiteralNode("0", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInteger))));

            // Add directories
            var testfolder = g.CreateUriNode(new Uri("https://localhost:44340/testfolder"));
            // Add link between root and folder
            g.Assert(n0, contains, testfolder);
            // Set types
            g.Assert(new Triple(testfolder, rdfType, basicContainer));
            g.Assert(new Triple(testfolder, rdfType, container));
            g.Assert(new Triple(testfolder, rdfType, rescoure));
            // Set values
            //g.Assert(new Triple(testfolder, modified, g.CreateLiteralNode("2018-11-20T12:25:08Z", "XML:dateTime")));
            g.Assert(new Triple(testfolder, mtime, g.CreateLiteralNode("1542716708", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInt))));
            g.Assert(new Triple(testfolder, size, g.CreateLiteralNode("0", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInteger))));

            /*
var g = new Graph();;
var ttlparser = new TurtleParser();

//Load using a Filename
ttlparser.Load(g, "Testvectors/directorylist.ttl");*/
            return StringWriter.Write(g, turtleWriter);

        }
    }
}
