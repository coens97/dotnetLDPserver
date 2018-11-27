using System.Collections.Generic;

namespace RDFCreator.DTO
{
    public class Graph
    {
        public Dictionary<string,string> Namespaces { get; set; }
        public GraphNode RootNode { get; set; }
    }
}
