namespace ShadowrunTracker.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using ShadowrunTracker.Api.Models;

    public class ChartHub : Hub
    {
        public async Task BroadcastChartData(List<ChartModel> data) =>
            await Clients.All.SendAsync("broadcastchartdata", data);
    }
}
