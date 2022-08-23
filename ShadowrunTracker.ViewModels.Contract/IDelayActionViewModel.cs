namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Model;

    public interface IDelayActionViewModel : IDisposableModalViewModel<IDelayActionViewModel>
    {
        IParticipantInitiativeViewModel Participant { get; }

        IReadOnlyCollection<DelayAction> Actions { get; }

        DelayAction? CurrentAction { get; set; }
    }
}
