using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Sebs.Api.Redis.Data.Entities;
using System.Net;

namespace Sebs.Api.Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public SampleController(
            IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{key}", Name = "GetSample")]
        [ProducesResponseType(typeof(SampleEntity), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SampleEntity>> GetSample(string key)
        {
            var sample = await _cache.GetStringAsync(key);
            return Ok(sample ?? "Default sample");
        }
    }
}