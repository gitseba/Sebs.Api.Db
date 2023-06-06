using MongoDB.Driver;
using Sebs.Api.Mongo.Data.Entities;

namespace Sebs.Api.Mongo.Data.Contexts
{
    public interface ISampleContext
    {
        public IMongoCollection<SampleEntity> Samples { get; }
    }
}
