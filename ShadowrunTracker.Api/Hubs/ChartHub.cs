namespace ShadowrunTracker.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using ShadowrunTracker.Api.Models;

    public class ChartHub : Hub<IChartClient>
    {
        public async Task BroadcastChartData(List<ChartModel> data)
        {
            var u = Context.User;
            var id = Context.UserIdentifier;
            var connId = Context.ConnectionId;
            await Clients.All.BroadcastChartData(data);
        }

        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }
    }
}
