using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi_Mvc5_MongoDb_POC.Models;

namespace WebApi_Mvc5_MongoDb_POC.Controllers
{
    public class SpeakerController : ApiController
    {
        
        public SpeakerController()
        {
        }

        private ISpeakerRespository GetRepository()
        {
            IEnumerable<string> values = null;
            if (Request.Headers.TryGetValues("repo", out values))
            {
                return values.FirstOrDefault() == "mongo"
                    ? WebApiConfig.MongoSpeakerRespository
                    : WebApiConfig.AzureDocDbSpeakerRespository;
            }
            return WebApiConfig.AzureDocDbSpeakerRespository;
        }

        [HttpGet]
        public async Task<IEnumerable<Speaker>> GetAll()
        {
            var enumerable = await GetRepository().AllSpeakers();
            return enumerable;
        }

        [HttpGet]
        public async Task<Speaker> GetById(string id)
        {
            var item = await GetRepository().GetById(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateSpeaker([FromBody] Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "AzureDocDbSpeaker failed validation");
            }
            else
            {
                await GetRepository().Add(speaker);

                var response = Request.CreateResponse(HttpStatusCode.Created, speaker);
                string uri = Url.Link("DefaultApi", new {id = speaker.DbId});
                response.Headers.Location = new Uri(uri);
                return response;

                //string url = Url.RouteUrl("GetByIdRoute", new { id = AzureDocDbSpeaker.Id.ToString() }, Request.Scheme, Request.Host.ToUriComponent());
                //HttpContext.Current.Response.StatusCode = 201;
                //HttpContext.Current.Response.Headers["Location"] = url;
            }
        }
        
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateSpeaker([FromBody] Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Speaker failed validation");
            }
            else
            {
                await GetRepository().Update(speaker);

                var response = Request.CreateResponse(HttpStatusCode.OK, speaker);
                string uri = Url.Link("DefaultApi", new {id = speaker.DbId});
                response.Headers.Location = new Uri(uri);
                return response;

                //string url = Url.RouteUrl("GetByIdRoute", new { id = AzureDocDbSpeaker.Id.ToString() }, Request.Scheme, Request.Host.ToUriComponent());
                //HttpContext.Current.Response.StatusCode = 201;
                //HttpContext.Current.Response.Headers["Location"] = url;
            }
        }

        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            return await GetRepository().Remove(id);
        }

    }
}
