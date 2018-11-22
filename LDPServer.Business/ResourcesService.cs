
using LDPServer.Common.DTO;
using LDPServer.Common.Interfaces;
using System;

namespace LDPServer.Business
{
    public class ResourcesService
    {
        public RdfService _rdfService;
        public IResourceRepository _rescourceRepository;

        public ResourcesService(RdfService rdfService, IResourceRepository rescourceRepository)
        {
            _rdfService = rdfService;
            _rescourceRepository = rescourceRepository;
        }

        public Tuple<string, int> GetDirectoryRescources(string path)
        {
            var directoryResult = _rescourceRepository.GetRescourcesOfDirectory(path);
            
            if (!directoryResult.Exists)
            {
                return new Tuple<string, int>("Can't find folder", 404);
            }

            return new Tuple<string,int>(_rdfService.RescourcesToText("https://localhost:44340/" + path, directoryResult), 200);
        }

        public string CreateDirectory(string path, string foldername)
        {
            return _rescourceRepository.CreateDirectory(path, foldername);
        }

        public string CreateFile(string path, string filename)
        {
            return _rescourceRepository.CreateFile(path, filename);
        }

        public void DeleteResource(string path)
        {
            _rescourceRepository.DeleteResource(path);
        }
    }
}
