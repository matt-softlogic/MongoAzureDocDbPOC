using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace WebApi_Mvc5_MongoDb_POC.Models
{
    public class Speaker
    {
        public virtual string DbId { get; set; }

        public virtual string email { get; set; }

        public virtual string first { get; set; }

        public virtual string last { get; set; }

        public virtual string twitter { get; set; }

        public virtual string title { get; set; }

        public virtual string blog { get; set; }
    }


    public partial class AzureDocDbSpeaker : Speaker
    {
        [JsonProperty(PropertyName = "id")]
        public override string DbId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public override string email { get; set; }

        [JsonProperty("first")]
        public override string first { get; set; }

        [JsonProperty("last")]
        public override string last { get; set; }

        [JsonProperty("twitter")]
        public override string twitter { get; set; }

        [JsonProperty("title")]
        public override string title { get; set; }

        [JsonProperty("blog")]
        public override string blog { get; set; }

    }  
    
    public partial class MongoSpeaker : Speaker
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public new ObjectId DbId { get; set; }

        [BsonElement("email")]
        public override string email { get; set; }

        [BsonElement("first")]
        public override string first { get; set; }

        [BsonElement("last")]
        public override string last { get; set; }

        [BsonElement("twitter")]
        public override string twitter { get; set; }

        [BsonElement("title")]
        public override string title { get; set; }

        [BsonElement("blog")]
        public override string blog { get; set; }

    }

        /*
         * BSON ObjectID is a 12-byte value consisting of:
         * - a 4-byte timestamp (seconds since epoch)
         * - a 3-byte machine id
         * - a 2-byte process id
         * - a 3-byte counter
         * 
         * 0123 456     78  91011
         * time machine pid inc
         */
        public class ObjectIdConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ObjectId);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType != JsonToken.String)
                {
                    throw new Exception(
                        String.Format("Unexpected token parsing ObjectId. Expected String, got {0}.",
                                      reader.TokenType));
                }

                var value = (string)reader.Value;
                return String.IsNullOrEmpty(value) ? ObjectId.Empty : new ObjectId(value);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is ObjectId)
                {
                    var objectId = (ObjectId)value;

                    writer.WriteValue(objectId != ObjectId.Empty ? objectId.ToString() : String.Empty);
                }
                else
                {
                    throw new Exception("Expected ObjectId value.");
                }
            }
        }
    
}
