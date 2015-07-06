using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi_Mvc5_MongoDb_POC.Models;

namespace WebApi_Mvc5_MongoDb_POC
{
    public class SpeakerMongoRepository : ISpeakerRespository
    {
        private readonly IMongoDatabase _database;
        private readonly Settings _settings;

        public SpeakerMongoRepository(Settings settings)
        {
            _settings = settings;
            _database = Connect();
        }

        public async Task Add(Speaker speaker)
        {
            var mongoSpeaker = speaker as MongoSpeaker;
            await GetSpeakerCollection().InsertOneAsync(mongoSpeaker);
        }

        public async Task Update(Speaker speaker)
        {
            var mongoSpeaker = speaker as MongoSpeaker;
            await GetSpeakerCollection().ReplaceOneAsync(speaker1 => speaker1.DbId == mongoSpeaker.DbId, mongoSpeaker);
        }

        public async Task<IEnumerable<Speaker>> AllSpeakers()
        {
            return await GetSpeakerCollection().Find(_ => true).ToListAsync();
        }

        public async Task<Speaker> GetById(string id)
        {
            var speakers = await GetSpeakerCollection().Find(GetSpeakerByIdFilter(id)).ToListAsync();
            return speakers.FirstOrDefault();
        }

        public async Task<bool> Remove(string id)
        {
            await GetSpeakerCollection().DeleteOneAsync(GetSpeakerByIdFilter(id));
            return await GetById(id) == null;
        }

        private IMongoDatabase Connect()
        {
            var client = new MongoClient(_settings.MongoConnection);
            //var server = client.GetServer();
            return client.GetDatabase(_settings.Database);
        }

        private IMongoCollection<MongoSpeaker> GetSpeakerCollection()
        {
            return _database.GetCollection<MongoSpeaker>("speakers");
        }

        private static FilterDefinition<MongoSpeaker> GetSpeakerByIdFilter(string id)
        {
            var objId = new ObjectId(id);
            var filter = Builders<MongoSpeaker>.Filter.Eq(speaker => speaker.DbId, objId);
            return filter;
        }
    }
}