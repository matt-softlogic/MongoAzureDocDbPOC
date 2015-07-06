using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi_Mvc5_MongoDb_POC.Models;

namespace WebApi_Mvc5_MongoDb_POC
{
    public interface ISpeakerRespository
    {
        Task<IEnumerable<Speaker>> AllSpeakers();

        Task<Speaker> GetById(string id);

        Task Add(Speaker speaker);

        Task Update(Speaker speaker);

        Task<bool> Remove(string id);
    }
}
