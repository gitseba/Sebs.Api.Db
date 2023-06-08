using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sebs.Api.SqlServer.Data.Contexts;
using Sebs.Api.SqlServer.Data.Entities;
using System.Net;

namespace Sebs.Api.PostgreSQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly SampleContext _dbContext;

        public SampleController(SampleContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SampleEntity>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<SampleEntity>>> GetSamples()
        {
            //ToDo Add repository for getting data
            try
            {
                var samples = await _dbContext.Samples.ToListAsync();
                return Ok(samples);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}