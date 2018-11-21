
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
    }
}
