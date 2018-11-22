using LDPServer.Business;
using LDPServer.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace LDPServer.Presentation.Controllers
{
    public class ResourceController : ApiController
    {
        private ResourcesService _rdfRescource;
        private IDataFolder _dataFolder;

        public ResourceController(ResourcesService rdfRescource, IDataFolder dataFolder)
        {
            _rdfRescource = rdfRescource;
            _dataFolder = dataFolder;
        }

        /// <summary>
        /// For GET request on any directory or file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Index()
        {
            // Relative path 
            var relativePath = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.AbsolutePath.Substring(1)); 

            if (relativePath.EndsWith("/") || string.IsNullOrWhiteSpace(relativePath)) // If directory
            {
                return GetDirectoryRescources(relativePath);
            }
            else // If request file
            {
                return GetFileDownload(relativePath);
            }
        }

        private HttpResponseMessage GetDirectoryRescources(string relativePath)
        {
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

        private HttpResponseMessage GetFileDownload(string relativePath)
        {
            var fileName = Path.GetFileName(relativePath);
            var filePath = _dataFolder.GetDataFolder() + relativePath;

            if (!File.Exists(filePath))
            {   // Respond a 404
                return Request.CreateResponse(HttpStatusCode.NotFound, "Rescource not found");
            }

            // Read file content
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(File.ReadAllBytes(filePath))
            };

            // Set headers
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}
