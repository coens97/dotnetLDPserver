using System;

namespace RDFCreator.DTO
{
    public class GraphNode
    {
        public string Uri { get; set; }
        public string[] Types { get; set; }
        public Tuple<string, string>[] NodeValues { get; set; }
        public GraphNode[] Children { get; set; }
    }
}
