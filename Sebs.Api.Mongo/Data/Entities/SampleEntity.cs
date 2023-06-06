using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Sebs.Api.Mongo.Data.Entities
{
    public class SampleEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }
    }
}
