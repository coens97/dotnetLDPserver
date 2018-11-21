
using LDPServer.Common.DTO;

namespace LDPServer.Business
{
    public class RescourcesService
    {
        public RdfService _rdfService;
        public RescourcesService(RdfService rdfService)
        {
            _rdfService = rdfService;
        }

        public string GetDirectoryRescources(string path)
        {
            // TODO: Use IRescoureRepository to get list of rescources
            return _rdfService.RescourcesToText("https://localhost:44340/", new RescourcesDirectory
            {
                RootDirectory = new RescourceMetaData
                {
                    LastModificationTime = 1542716708,
                    Size = 0,
                    IsDirectory = true,
                },
                Rescources = new[] {
                    new RescourceMetaData
                    {
                       Name = "testfolder",
                       LastModificationTime = 1542716708,
                       Size = 0,
                       IsDirectory = true,
                    },
                    new RescourceMetaData
                    {
                       Name = "music",
                       LastModificationTime = 1542716758,
                       Size = 0,
                       IsDirectory = true,
                    }
                }
            });
        }
    }
}
