using System.Configuration;
using System.Web.Http;
using WebApi_Mvc5_MongoDb_POC.Models;

namespace WebApi_Mvc5_MongoDb_POC
{
    public static class WebApiConfig
    {
        public static Settings Settings = new Settings();
        public static ISpeakerRespository MongoSpeakerRespository;
        public static ISpeakerRespository AzureDocDbSpeakerRespository;

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});

            //Automapper Bootstrap
            SpeakerMapper.Configure();

            Settings.Database = ConfigurationManager.AppSettings["database"];
            Settings.MongoConnection = ConfigurationManager.AppSettings["mongoconnection"];
            Settings.Repository = ConfigurationManager.AppSettings["repo"];

            MongoSpeakerRespository = new SpeakerMongoRepository(Settings);
            AzureDocDbSpeakerRespository = new SpeakerAzureDocDbRepository(Settings);
        }
    }
}