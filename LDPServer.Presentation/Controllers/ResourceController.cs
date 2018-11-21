using LDPServer.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace LDPServer.Presentation.Controllers
{
    public class ResourceController : ApiController
    {
        private ResourcesService _rdfRescource;

        public ResourceController(ResourcesService rdfRescource)
        {
            _rdfRescource = rdfRescource;
        }

        [HttpGet]
        public HttpResponseMessage Index()
        {
            var relativePath = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.AbsolutePath.Substring(1)); 
            var result = _rdfRescource.GetDirectoryRescources(relativePath);
            var response = Request.CreateResponse((HttpStatusCode)result.Item2);
            response.Content = new StringContent(result.Item1, Encoding.UTF8, "text/turtle");
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
