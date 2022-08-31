namespace ShadowrunTracker.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Caching.Memory;
    using ShadowrunTracker.Communication;

    public class EncounterHub : Hub<IEncounterClient>
    {
        private readonly IMemoryCache _cache;

        public EncounterHub(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task SendUpdate(Update update, string? player)
        {
            _ = _cache.TryGetValue(Constants.EncounterKey, out var _);
            if (player != null)
            {
                await Clients.Group(player).ReceiveUpdate(update);
            }
            else if (update.SessionName is null)
            {
                await Clients.Others.ReceiveUpdate(update);
            }
            else
            {
                await Clients.OthersInGroup(update.SessionName).ReceiveUpdate(update);
            }
        }

        public async Task AddToGroup(string group)
        {
            if (group != Constants.GmGroupName)
            {
                var encounters = _cache.GetOrCreate(Constants.EncounterKey, e => new HashSet<string>());
                encounters.Add(group);
                _cache.Set(Constants.EncounterKey, encounters);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public async Task RequestState(string sessionName, string user)
        {
            await Clients.Group(Constants.GmGroupName).RequestState(sessionName, user);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext is null)
            {
                throw new InvalidOperationException("Cannot get HttpContext");
            }
            var username = httpContext.Request.Headers["username"].Single();
            await Groups.AddToGroupAsync(Context.ConnectionId, username);
        }
    }
}
