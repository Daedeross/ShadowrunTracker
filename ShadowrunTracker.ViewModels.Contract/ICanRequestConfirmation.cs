namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;

    public interface ICanRequestConfirmation
    {
        Interaction<string, bool> ConfirmationRequest { get; }
    }
}
