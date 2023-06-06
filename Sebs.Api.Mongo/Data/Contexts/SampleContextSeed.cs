using MongoDB.Driver;
using Sebs.Api.Mongo.Data.Entities;

namespace Sebs.Api.Mongo.Data.Contexts
{
    /// <summary>
    /// Default dummy values to seed the mongo db
    /// </summary>
    public class SampleContextSeed
    {
        public static void SeedData(IMongoCollection<SampleEntity> samples)
        {
            bool anySample = samples.Find(p => true).Any();
            if (!anySample)
            {
                samples.InsertManyAsync(GetSampleProducts());
            }
        }

        private static IEnumerable<SampleEntity> GetSampleProducts()
        {
            return new List<SampleEntity>()
            {
                new SampleEntity()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "A"
                },
                new SampleEntity()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "B"
                },
                new SampleEntity()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "C"
                }
            };
        }
    }
}