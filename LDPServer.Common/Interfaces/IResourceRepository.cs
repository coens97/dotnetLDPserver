using LDPServer.Common.DTO;

namespace LDPServer.Common.Interfaces
{
    public interface IResourceRepository
    {
        ResourcesDirectory GetRescourcesOfDirectory(string path);
        string CreateDirectory(string path, string folderName);
        string CreateFile(string path, string fileName);
        void DeleteResource(string path);
    }
}
