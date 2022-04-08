using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace ShadowrunTracker.ViewModels
{
    public class CanRequestConfirmationBase : ViewModelBase, ICanRequestConfirmation
    {
        protected readonly SerialDisposable _confirmationSubscription;

        public CanRequestConfirmationBase()
        {
            ConfirmationRequest = new Interaction<string, bool>();
            _confirmationSubscription = new SerialDisposable();
            _disposables.Add(_confirmationSubscription);
        }

        public Interaction<string, bool> ConfirmationRequest { get; }

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
