namespace ShadowrunTracker.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using ShadowrunTracker.Api.DataStorage;
    using ShadowrunTracker.Api.Hubs;
    using ShadowrunTracker.Api.TimerFeatures;

    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IHubContext<ChartHub> _hub;
        private readonly TimerManager _timer;

        public ChartController(IHubContext<ChartHub> hub, TimerManager timer)
        {
            _hub = hub;
            _timer = timer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (!_timer.IsTimerStarted)
            {
                _timer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferChartData", DataManager.GetData()));
            }

            return Ok(new { Message = "Request Completer" });
        }
    }
}
