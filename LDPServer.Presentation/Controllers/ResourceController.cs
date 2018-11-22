using LDPServer.Business;
using LDPServer.Common.DTO;
using LDPServer.Common.Interfaces;
using System;
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
        private ResourcesService _resourceService;
        private IDataFolder _dataFolder;

        public ResourceController(ResourcesService resourceService, IDataFolder dataFolder)
        {
            _resourceService = resourceService;
            _dataFolder = dataFolder;
        }

        #region GET Requests
        /// <summary>
        /// For GET request on any directory or file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Index()
        {
            // Relative path 
            var relativePath = GetRelativePath();

            if (IsDirectory(relativePath))
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
            var result = _resourceService.GetDirectoryRescources(relativePath);
            var response = Request.CreateResponse((HttpStatusCode)result.Item2);
            response.Content = new StringContent(result.Item1, Encoding.UTF8, "text/turtle");

            AddCorsHeaders(response);

            return response;
        }

        private HttpResponseMessage GetFileDownload(string relativePath)
        {
            var fileName = Path.GetFileName(relativePath);
            var filePath = _dataFolder.GetDataFolder() + relativePath;

            if (!File.Exists(filePath))
            {   // Respond a 404
                return Request.CreateResponse(HttpStatusCode.NotFound, "Resource not found");
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
        #endregion

        [ActionName("Index")]
        [HttpPost]
        public HttpResponseMessage IndexPost()
        {
            var files = HttpContext.Current.Request.Files;
            var relativePath = GetRelativePath();

            if (Request.Headers.Contains("Link") && Request.Headers.Contains("Slug"))
            {
                var response = Request.CreateResponse(HttpStatusCode.Created);
                response.Content = new StringContent("Created", Encoding.UTF8, "text/plain");
                AddCorsHeaders(response);
                var resourceName = Request.Headers.GetValues("Slug").First();

                var link = Request.Headers.GetValues("Link").First();

                var location = ""; // path to new rescource
                if (link.Contains("ldp#BasicContainer")) // If creating directory
                {
                    // If directory already exists, a new directory name is generated
                    location = _resourceService.CreateDirectory(relativePath, resourceName) + "/";
                }
                else if (link.Contains("ldp#Resource"))
                {
                    location = _resourceService.CreateFile(relativePath, resourceName);
                }
                response.Headers.Add("Location", location);
                return response;
            }
            else if(files.Count > 0)
            {
                var filesCollection = Enumerable.Range(0, files.Count)
                    .Select(x => new UploadFile {
                        FileName = files[x].FileName,
                        SaveAs = files[x].SaveAs
                    });

                _resourceService.UploadFiles(filesCollection, relativePath);
                // Return OK
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("OK", Encoding.UTF8, "text/plain");
                return response;
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing Link and Slug headers or attachment");
        }

        /// <summary>
        /// HEAD requests the headers that are returned if the specified resource 
        /// would be requested with an HTTP GET method
        /// </summary>
        /// <returns></returns>
        [ActionName("Index")]
        [HttpHead]
        public HttpResponseMessage IndexHead()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);

            AddCorsHeaders(response);
            AddAllowHeader(response);
            return response;
        }

        /// <summary>
        /// Options request is used for security, to check which methods and headers are allowed
        /// </summary>
        /// <returns></returns>
        [ActionName("Index")]
        [HttpOptions]
        public HttpResponseMessage IndexOptions()
        {
            var response = Request.CreateResponse(HttpStatusCode.NoContent);

            AddCorsHeaders(response);
            AddAllowHeader(response);
            return response;
        }


        /// <summary>
        /// For DELETE request on any directory or file
        /// </summary>
        /// <returns></returns>
        [ActionName("Index")]
        [HttpDelete]
        public HttpResponseMessage IndexDelete()
        {
            // Relative path 
            var relativePath = GetRelativePath();

            _resourceService.DeleteResource(relativePath);
            // Return OK
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("OK", Encoding.UTF8, "text/plain");
            return response;
        }

        /// <summary>
        /// Add CORS headers based on the referer URL, because of security reasons
        /// thw * wildcard can't be used
        /// </summary>
        /// <param name="response"></param>
        private void AddCorsHeaders(HttpResponseMessage response)
        {
            response.Headers.Add("Access-Control-Allow-Credentials", "true");

            if (Request.Headers.Contains("Origin"))
            {
                var uri = new Uri(Request.Headers.GetValues("Origin").First());
                response.Headers.Add("Access-Control-Allow-Origin", uri.GetLeftPart(UriPartial.Authority));
            }
            else
            {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
        }

        private void AddAllowHeader(HttpResponseMessage response)
        {
            response.Headers.Add("Access-Control-Allow-Headers", "content-type,link,slug");
            response.Headers.Add("Access-Control-Allow-Methods", "OPTIONS,HEAD,GET,PATCH,POST,PUT,DELETE");
        }

        /// <summary>
        /// Retrieves relative path from request URL
        /// </summary>
        /// <returns>Relative path</returns>
        private string GetRelativePath()
        {
            return HttpUtility.UrlDecode(HttpContext.Current.Request.Url.AbsolutePath.Substring(1));
        }

        private bool IsDirectory(string relativePath)
        {
            return relativePath.EndsWith("/") || string.IsNullOrWhiteSpace(relativePath);
        }
    }
}
