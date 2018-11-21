using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPServer.Common.DTO
{
    public class ResourcesDirectory
    {
        /// <summary>
        /// Whetever directory exists
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Folder that contains the rescources
        /// </summary>
        public ResourceMetaData RootDirectory { get; set; }

        /// <summary>
        /// List of rescources of directory
        /// </summary>
        public IEnumerable<ResourceMetaData> Rescources { get; set; }
    }
}
