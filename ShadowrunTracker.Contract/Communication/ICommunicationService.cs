namespace ShadowrunTracker.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Service interface for .NET clients to communicate
    /// </summary>
    public interface ICommunicationService
    {
        bool Connected();

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        /// <returns></returns>
        Task DisconnectAsync();

        /// <summary>
        /// <see cref="IObservable{T}"/> of incomming <see cref="Update"/>s the client can subscribe to.
        /// Intended for player/dependent clients
        /// </summary>
        IObservable<Update> Incomming { get; }

        /// <summary>
        /// Observable which pushes a player name when said player requests a sync
        /// of all data in the encounter.
        /// </summary>
        IObservable<string> SyncRequest { get; }

        /// <summary>
        /// Send outgoing updates to other connected clients.
        /// Intended for GMs/controlling clients
        /// </summary>
        /// <param name="update">The <see cref="Update"/> to send.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task PushUpdateAsync(Update update, string? player = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of current session names.
        /// </summary>
        /// <returns>The <see cref="Task{T}"/> wich results in a list of strings.</returns>
        Task<IList<string>> GetCurrentSessionsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Connect to a current session.
        /// </summary>
        /// <param name="sessionName">The name of the session to connect to.
        /// Will create a session with that name if connectining as a GM.</param>
        /// <param name="asGM">True if connecting as a GM.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task ConnectToSession(string sessionName, bool asGM = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RequestStateAsync(CancellationToken cancellationToken);
    }
}
