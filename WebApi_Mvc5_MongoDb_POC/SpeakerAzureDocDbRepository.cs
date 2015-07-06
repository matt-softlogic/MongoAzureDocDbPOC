using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using WebApi_Mvc5_MongoDb_POC.Models;

namespace WebApi_Mvc5_MongoDb_POC
{
    public class SpeakerAzureDocDbRepository : ISpeakerRespository
    {
        private Settings _settings;

        private static DocumentClient _client;

        //Assign a id for your database & collection 
        private static readonly string databaseId = ConfigurationManager.AppSettings["ADDBDatabaseId"];
        private static readonly string collectionId = ConfigurationManager.AppSettings["ADDBCollectionId"];

        //Read the DocumentDB endpointUrl and authorisationKeys from config
        //These values are available from the Azure Management Portal on the DocumentDB Account Blade under "Keys"
        //NB > Keep these values in a safe & secure location. Together they provide Administrative access to your DocDB account
        private static readonly string endpointUrl = ConfigurationManager.AppSettings["ADDBEndPointUrl"];
        private static readonly string authorizationKey = ConfigurationManager.AppSettings["ADDBAuthorizationKey"];
        private Database _database;
        private DocumentCollection _collection;

        public SpeakerAzureDocDbRepository(Settings settings)
        {
            _settings = settings;

            _client = new DocumentClient(new Uri(endpointUrl), authorizationKey);
            
            //Get, or Create, the Database
            _database = GetOrCreateDatabaseAsync(databaseId).Result;

            //Get, or Create, the Document Collection
            _collection = GetOrCreateCollectionAsync(_database.SelfLink, collectionId).Result;
        }


        public async Task<IEnumerable<Speaker>> AllSpeakers()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var speakers = _client.CreateDocumentQuery<AzureDocDbSpeaker>(_collection.SelfLink);
                return speakers.ToArray();
            });
            return await task;
        }

        public async Task<Speaker> GetById(string id)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var speakers = _client.CreateDocumentQuery<AzureDocDbSpeaker>(_collection.SelfLink)
                                      .Where(speaker => speaker.DbId.ToString() == id);
                return speakers.FirstOrDefault();
            });
            return await task;
        }

        public async Task Add(Speaker speaker)
        {
            var result = await _client.CreateDocumentAsync(_collection.SelfLink, speaker);
            
        }

        public async Task Update(Speaker speaker)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Database by id, or create a new one if one with the id provided doesn't exist.
        /// </summary>
        /// <param name="id">The id of the Database to search for, or create.</param>
        /// <returns>The matched, or created, Database object</returns>
        private static async Task<Database> GetOrCreateDatabaseAsync(string id)
        {
            Database database = _client.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault();
            if (database == null)
            {
                database = await _client.CreateDatabaseAsync(new Database { Id = id });
            }

            return database;
        }

        /// <summary>
        /// Get a DocuemntCollection by id, or create a new one if one with the id provided doesn't exist.
        /// </summary>
        /// <param name="dbLink"></param>
        /// <param name="id">The id of the DocumentCollection to search for, or create.</param>
        /// <returns>The matched, or created, DocumentCollection object</returns>
        private static async Task<DocumentCollection> GetOrCreateCollectionAsync(string dbLink, string id)
        {
            DocumentCollection collection = _client.CreateDocumentCollectionQuery(dbLink).Where(c => c.Id == id).ToArray().FirstOrDefault();
            if (collection == null)
            {
                collection = await _client.CreateDocumentCollectionAsync(dbLink, new DocumentCollection { Id = id });
            }

            return collection;
        }
    }
}
