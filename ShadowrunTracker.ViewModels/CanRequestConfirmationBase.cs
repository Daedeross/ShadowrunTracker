namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Utils;
    using System;
    using System.Reactive.Disposables;

    public class CanRequestConfirmationBase : ViewModelBase, ICanRequestConfirmation
    {
        protected readonly SerialDisposable _confirmationSubscription;

        public CanRequestConfirmationBase()
        {
            _confirmationSubscription = new SerialDisposable();
            _disposables.Add(_confirmationSubscription);
        }

        public Interaction<string, bool> ConfirmationRequest => Interactions.ConfirmationRequest;

        protected void RequestConfirmation(string message, Action<bool> handler)
        {
            _confirmationSubscription.Disposable = ConfirmationRequest
                .Handle(message)
                .Subscribe(handler);
        }

        protected void RequestConfirmation(string message, Action onOk, Action? onCancel = null)
        {
            RequestConfirmation(message, ok => { if (ok) { onOk(); } else { onCancel?.Invoke(); } });
        }
    }
}
