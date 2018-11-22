using LDPServer.Common.DTO;
using System.Collections.Generic;

namespace LDPServer.Common.Interfaces
{
    public interface IResourceRepository
    {
        ResourcesDirectory GetRescourcesOfDirectory(string path);
        string CreateDirectory(string path, string folderName);
        string CreateFile(string path, string fileName);
        void DeleteResource(string path);
        void UploadFiles(IEnumerable<UploadFile> files, string relativePath);
    }
}
