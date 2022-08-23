using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowrunTracker.Client
{
    public class EncounterHubClient
    {
        private void Foo()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:53353/ChatHub")
                .WithAutomaticReconnect()
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        public async Task ConnectAsync()
        {

        }
    }
}
