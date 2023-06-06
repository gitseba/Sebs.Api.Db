using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Sebs.Api.PostgreSQL.Data.Entity;
using System.Net;

namespace Sebs.Api.PostgreSQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SampleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SampleEntity>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<SampleEntity>>> GetSamples()
        {
            //ToDo Add repository for getting data
            try
            {
                using (var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString")))
                {
                    var samples = await connection.QueryAsync<SampleEntity>("SELECT * FROM  Samples");

                    if (samples == null)
                        return new List<SampleEntity> { new SampleEntity { Name = "Sample name", Amount = 100, Description = "Sample Description" } };

                    return Ok(samples);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}