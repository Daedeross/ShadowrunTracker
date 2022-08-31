namespace ShadowrunTracker.Client
{
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using ShadowrunTracker.Communication;
    using ShadowrunTracker.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Json;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    public class CommunicationService : ICommunicationService, IAsyncDisposable
    {
        private readonly HttpClient _http;
        private readonly IOptionsSnapshot<WebOptions> _options;
        private readonly string _username;
        private readonly HubConnection _hubConnection;
        private readonly Subject<Update> _incomingQueue;
        private readonly Subject<string> _requestState;

        private bool _disposedValue;
        private string? _sessionName;

        public CommunicationService(HttpClient http, IRetryPolicy retryPolicy, IOptionsSnapshot<WebOptions> options, string userName)
        {
            _http = http;
            _options = options;
            _username = userName;
            _incomingQueue = new Subject<Update>();
            _requestState = new Subject<string>();

            var url = $"{_options.Value.HubRoot}/encounter";

            _hubConnection = new HubConnectionBuilder()
                .AddJsonProtocol(o => o.PayloadSerializerOptions.Converters.Add(new RecordJsonConverter()))
                .WithUrl(url, o =>
                {
                    o.Headers.Add("username", userName);
                })
                .WithAutomaticReconnect(retryPolicy)
                .Build();

#if DEBUG
            _hubConnection.ServerTimeout = TimeSpan.FromMinutes(5);
#endif
        }

        public bool Connected() => _hubConnection.State != HubConnectionState.Disconnected;

        public async Task ConnectAsync()
        {
            if (_hubConnection.State != HubConnectionState.Disconnected)
            {
                return;
            }
            _hubConnection.On<Update>(HubMethods.ReceiveUpdate, OnReceiveUpdate);
            _hubConnection.On<string, string>(HubMethods.RequestState, OnRequestState);

            await _hubConnection.StartAsync();
        }

        public async Task DisconnectAsync()
        {
            await _hubConnection.StopAsync();
        }

        public void OnReceiveUpdate(Update update)
        {
            _incomingQueue.OnNext(update);
        }

        private void OnRequestState(string session, string player)
        {
            if (string.Equals(session, _sessionName))
            {
                _requestState.OnNext(player);
            }
        }

        public IObservable<Update> Incomming => _incomingQueue;

        public IObservable<string> SyncRequest => _requestState;

        public async Task PushUpdateAsync(Update update, CancellationToken cancellationToken = default)
        {
            if (Connected())
            {
                await _hubConnection.InvokeAsync(HubMethods.SendUpdate, update, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task PushUpdateAsync(Update update, string? player = null, CancellationToken cancellationToken = default)
        {
            if (Connected())
            {
                await _hubConnection.InvokeAsync(HubMethods.SendUpdate, update, player, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IList<string>> GetCurrentSessionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _http.GetAsync($"{_options.Value.ApiRoot}/encounter", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Encounters>(options: null, cancellationToken);
                return result?.Names ?? new List<string>();
            }
            else
            {
                throw new TimeoutException("Web request timed out");
            }
        }

        public async Task ConnectToSession(string sessionName, bool asGM = false, CancellationToken cancellationToken = default)
        {
            if (Connected())
            {
                _sessionName = sessionName;

                if (asGM)
                {
                    await _hubConnection.InvokeAsync(HubMethods.AddToGroup, Constants.GmGroupName, cancellationToken);
                }
                await _hubConnection.InvokeAsync(HubMethods.AddToGroup, sessionName, cancellationToken);
            }
        }

        public async Task RequestStateAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_sessionName))
            {
                throw new InvalidOperationException("Cannot get state until connected to a session");
            }
            if (Connected())
            {
                await _hubConnection.InvokeAsync(HubMethods.RequestState, _sessionName, _username, cancellationToken);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _ = _hubConnection.DisposeAsync();
                }

                _disposedValue = true;
            }
        }

        public virtual async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
