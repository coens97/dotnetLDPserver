using LDPServer.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace LDPServer.Presentation.Controllers
{
    public class DirectoryController : ApiController
    {
        private RdfDirectory _rdfDirectory;

        public DirectoryController(RdfDirectory rdfDirectory)
        {
            _rdfDirectory = rdfDirectory;
        }

        [HttpGet]
        public HttpResponseMessage Index()
        {
            var result = _rdfDirectory.GetDirectory("/");
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Content = new StringContent(result, Encoding.UTF8, "text/turtle");
            response.Headers.Add("Access-Control-Allow-Credentials", "true");

            if (Request.Headers.Contains("Referer"))
            {
                var uri = new Uri(Request.Headers.GetValues("Referer").First());
                response.Headers.Add("Access-Control-Allow-Origin", uri.GetLeftPart(UriPartial.Authority));
            }
            else
            {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            return response;
        }
    }
}
