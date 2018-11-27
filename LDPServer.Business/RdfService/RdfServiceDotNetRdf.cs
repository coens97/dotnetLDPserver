using LDPServer.Common.DTO;
using LDPServer.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace LDPServer.Business.RdfService
{
    public class RdfServiceDotNetRdf : IRdfService
    {
        public string RescourcesToText(string baseUri, ResourcesDirectory rescource)
        {
            // Output format
            var rdfWriter = new CompressingTurtleWriter();

            // Empty graph
            var g = new Graph();
            var usedFileExtensions = new HashSet<string>();

            // Set current working directory
            g.BaseUri = new Uri(baseUri);

            // Clear RDF namespaces
            g.NamespaceMap.Clear();

            // Add LDP namespaces
            g.NamespaceMap.AddNamespace("n0", new Uri(baseUri)); // Root of graph
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

            Trace.Assert(rescource.RootDirectory.IsDirectory, "Root directory is not directory");
            g.Assert(new Triple(n0, rdfType, basicContainer)); // Set type as directory/container
            g.Assert(new Triple(n0, rdfType, container));

            // Set values of root directory
            var rootCreationTimeNode = g.CreateLiteralNode(
                rescource.RootDirectory.LastModificationTime.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInt));
            g.Assert(new Triple(n0, mtime, rootCreationTimeNode));
            var rootSizeNode = g.CreateLiteralNode(
                rescource.RootDirectory.Size.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInteger));
            g.Assert(new Triple(n0, size, rootSizeNode));

            // Add other rescources
            foreach (var iterRescource in rescource.Resources)
            {
                var uri = new Uri(baseUri + iterRescource.Name);

                // Add namespace for uri 
                //g.NamespaceMap.AddNamespace(iterRescource.Name, new Uri(baseUri));

                var newRescourceNode = g.CreateUriNode(uri);

                // Add link between root and folder
                g.Assert(n0, contains, newRescourceNode);

                // Set types
                if (iterRescource.IsDirectory)
                {
                    g.Assert(new Triple(newRescourceNode, rdfType, basicContainer));
                    g.Assert(new Triple(newRescourceNode, rdfType, container));
                }
                else
                {
                    // Is file
                    // If new file extension, add namespace
                    var fileExtension = Path.GetExtension(iterRescource.Name);
                    if (!string.IsNullOrWhiteSpace(fileExtension))
                    {
                        if (usedFileExtensions.Add(fileExtension))
                        {
                            string mimeType = MimeMapping.GetMimeMapping(iterRescource.Name);
                            g.NamespaceMap.AddNamespace(fileExtension, new Uri($"http://www.w3.org/ns/iana/media-types/{mimeType}#"));
                        }
                        // Link rescource type to file
                        var rescourceType = g.CreateUriNode(fileExtension + ":Resource");
                        g.Assert(new Triple(newRescourceNode, rdfType, rescourceType));
                    }

                }
                g.Assert(new Triple(newRescourceNode, rdfType, rescoure));

                // Set other metadata
                var newRescourceCreationTimeNode = g.CreateLiteralNode(
                    iterRescource.LastModificationTime.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInt));
                g.Assert(new Triple(newRescourceNode, mtime, newRescourceCreationTimeNode));
                var newRescourceSizeNode = g.CreateLiteralNode(
                    iterRescource.Size.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInteger));
                g.Assert(new Triple(newRescourceNode, size, newRescourceSizeNode));
            }

            return VDS.RDF.Writing.StringWriter.Write(g, rdfWriter);
        }
    }
}
