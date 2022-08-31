namespace ShadowrunTracker.Communication
{
    using System.Threading.Tasks;

    /// <summary>
    /// Methods the SignalR clients can listen to
    /// </summary>
    public interface IEncounterClient
    {
        Task ReceiveUpdate(Update update);

        Task RequestState(string sessionName, string user);
    }
}
