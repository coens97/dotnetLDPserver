
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
            return _rdfService.RescourcesToText("https://localhost:44340/", null);
        }
    }
}
