using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Sebs.Api.Mongo.Data.Entities;

namespace Sebs.Api.Mongo.Data.Contexts
{
    public class SampleContext : MongoClient, ISampleContext
    {
        public SampleContext(string connectionString, string databaseName, string collectionName) 
            : base(connectionString)
        {
            var mongoClient = new MongoClient(connectionString);

            //GetDatabase will create a database if Db does not exist
            var database = mongoClient.GetDatabase(databaseName);
            Samples = database.GetCollection<SampleEntity>(collectionName);
            
            SampleContextSeed.SeedData(Samples);
        }

        public IMongoCollection<SampleEntity> Samples { get; }
    }
}
