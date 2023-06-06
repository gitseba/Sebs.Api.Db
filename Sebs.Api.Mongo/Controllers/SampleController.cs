using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Sebs.Api.Mongo.Data.Contexts;
using Sebs.Api.Mongo.Data.Entities;
using System.Net;

namespace Sebs.Api.Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ISampleContext _dbContext;

        public SampleController(ISampleContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SampleEntity>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<SampleEntity>>> GetSamples()
        {
            //ToDo Add repository for getting data
            var samples = await _dbContext.Samples.Find(p => true).ToListAsync();
            return Ok(samples);
        }
    }
}
