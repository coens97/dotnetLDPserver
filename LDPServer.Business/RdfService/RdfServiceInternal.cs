using LDPServer.Common.DTO;
using LDPServer.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using RDFCreator.DTO;
using RDFCreator;

namespace LDPServer.Business.RdfService
{
    public class RdfServiceInternal : IRdfService
    {
        public string RescourcesToText(string baseUri, ResourcesDirectory resource)
        {
            // Initial graph
            var g = new Graph
            {
                // Initial namespaces
                Namespaces = new Dictionary<string, string>()
                {
                    { "", "/#" },
                    { "r", "" },
                    { "ldp", "http://www.w3.org/ns/ldp#" },
                    { "terms", "http://purl.org/dc/terms/" },
                    { "xml", "http://www.w3.org/2001/XMLSchema#" },
                    { "st", "http://www.w3.org/ns/posix/stat#" }
                },
                RootNode = ResourceToGraphNode(resource.RootDirectory)
            };

            var resourceList = new List<GraphNode>();
            // Add other rescources
            foreach (var iterRescource in resource.Resources)
            {
                if (iterRescource.IsDirectory)
                {
                    resourceList.Add(ResourceToGraphNode(iterRescource));
                }
                else
                {
                    // If file
                    var fileExtension = Path.GetExtension(iterRescource.Name);
                    if (string.IsNullOrWhiteSpace(fileExtension))
                    { // No file extension
                        fileExtension = null;
                    }
                    else
                    {
                        // Extension contains a dot
                        fileExtension = fileExtension.Substring(1);
                        if (!g.Namespaces.ContainsKey(fileExtension))
                        {
                            g.Namespaces.Add(fileExtension, MimeMapping.GetMimeMapping(iterRescource.Name));
                        }
                    }
                    resourceList.Add(ResourceToGraphNode(iterRescource, fileExtension));
                }
            }

            g.RootNode.Children = resourceList.ToArray();

            return new RDFTurtleWriter().ToText(g);
        }

        private GraphNode ResourceToGraphNode(ResourceMetaData resource, string fileExtension = null)
        {
            string[] types = null;
            // Root directory is not a resource
            if (resource.Name == "")
            {
                types = new[] { "ldp:BasicContainer", "ldp:Container" };
            }
            else if (resource.IsDirectory)
            {
                types = new[] { "ldp:BasicContainer", "ldp:Container" , "ldp:Resource" };
            }
            else
            {
                // Resource is file
                if (fileExtension == null)
                {
                    types = new[] { "ldp:Resource" };
                }
                else
                {
                    types = new[] { $"{fileExtension}:Resource", "ldp:Resource" } ;
                }
            }

            var timeXml = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(resource.LastModificationTime)
                .ToString("s") + "Z";

            return new GraphNode
            {
                Uri = resource.Name,
                Types = types,
                NodeValues = new Tuple<string, string>[]
                {
                    new Tuple<string, string>("terms:modified", $"\"{timeXml}\"^^XML:dateTime"),
                    new Tuple<string, string>("st:mtime", resource.LastModificationTime.ToString()),
                    new Tuple<string, string>("st:size", resource.Size.ToString())
                }
            };
        }
    }
}
