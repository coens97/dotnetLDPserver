using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPServer.Common.DTO
{
    public class RescourcesDirectory
    {
        /// <summary>
        /// Folder that contains the rescources
        /// </summary>
        public RescourceMetaData RootDirectory { get; set; }

        /// <summary>
        /// List of rescources of directory
        /// </summary>
        public IEnumerable<RescourceMetaData> Rescources { get; set; }
    }
}
