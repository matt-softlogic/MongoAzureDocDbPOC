using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;

namespace WebApi_Mvc5_MongoDb_POC.Models
{
    public static class SpeakerMapper
    {
        public static void Configure()
        {
            Mapper.CreateMap<List<ObjectId>, List<string>>().ConvertUsing(o => o.Select(os => os.ToString()).ToList());
            Mapper.CreateMap<List<string>, List<ObjectId>>().ConvertUsing(o => o.Select(os => ObjectId.Parse(os)).ToList());
            Mapper.CreateMap<ObjectId, string>().ConvertUsing(o => o.ToString());
            Mapper.CreateMap<string, ObjectId>().ConvertUsing(s => ObjectId.Parse(s));

            Mapper.CreateMap<Speaker, AzureDocDbSpeaker>()
                //.ForMember(x=>x.Id, opt => opt.MapFrom(src=>src.DbId))
                ;
            Mapper.CreateMap<AzureDocDbSpeaker, Speaker>();

            Mapper.CreateMap<Speaker, MongoSpeaker>()
                //.ForMember(x => x.Id, opt => opt.MapFrom(src => src.DbId))
                ;

            Mapper.AssertConfigurationIsValid();
        }
    }
}
