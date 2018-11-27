using RDFCreator.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFCreator
{
    public class RDFTurtleWriter
    {
        public string ToText(Graph graph)
        {
            var namespaces = graph.Namespaces.Select(x => $"@prefix {x.Key}: <{x.Value}>.");
            var nodes = NodesToText(graph.RootNode);

            var combined = namespaces.Concat(nodes);
            return string.Join(Environment.NewLine, combined);
        }

        private IEnumerable<string> NodesToText(GraphNode node)
        {
            // If root node name is r
            var name = node.Uri == string.Empty ? "r:" : "r:" + node.Uri;
            yield return $"{name}";
            
            if (node.Types.Any())
            {
                // Return 1 or more types
                yield return $"\ta {string.Join(", ", node.Types)};";
            }

            if (node.Children?.Any() == true)
            {
                // Refer to childrens identifier
                yield return "\tldp:contains";

                yield return string.Join($",{Environment.NewLine}",node.Children.Select(x => $"\t\t{name}{x.Uri}")) + ";";
            }

            // Can't directly return an IEnumerable
            foreach (var parameter in node.NodeValues.Select(x => $"\t{x.Item1} {x.Item2};"))
            {
                yield return parameter;
            }

            yield return "";

            if (node.Children?.Any() == true)
            {
                foreach (var child in node.Children)
                {
                    foreach (var line in NodesToText(child))
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}
