using LDPServer.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPServer.Common.Interfaces
{
    public interface IResourceRepository
    {
        ResourcesDirectory GetRescourcesOfDirectory(string path);
        string CreateDirectory(string path, string folderName);
    }
}
