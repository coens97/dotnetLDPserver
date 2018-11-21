using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPServer.Common.DTO
{
    public class ResourceMetaData
    {
        /// <summary>
        /// Name or relative path of rescource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Epoch value of when rescource was last modified
        /// </summary>
        public int LastModificationTime { get; set; }

        /// <summary>
        /// Size in bytes
        /// </summary>
        public int Size { get; set; }
        
        /// <summary>
        /// Is rescource a directory or container
        /// </summary>
        public bool IsDirectory { get; set; }
    }
}
