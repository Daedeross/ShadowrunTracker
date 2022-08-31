namespace ShadowrunTracker.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Caching.Memory;
    using ShadowrunTracker.Api.Hubs;
    using ShadowrunTracker.Communication;

    [Route("api/[controller]")]
    [ApiController]
    public class EncounterController : ControllerBase
    {
        private readonly IHubContext<EncounterHub, IEncounterClient> _hub;
        private readonly IMemoryCache _cache;

        private readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1))
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .SetPriority(CacheItemPriority.Normal);

        public EncounterController(IHubContext<EncounterHub, IEncounterClient> hub, IMemoryCache cache)
        {
            _hub = hub;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = _cache.GetOrCreate(Constants.EncounterKey, entry => new HashSet<string>()).ToList();
            //var list = new List<string> { "foobar" };

            return Ok(new Encounters { Names = list });
        }
    }
}
