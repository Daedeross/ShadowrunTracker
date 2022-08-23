namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using System;
    using System.Windows.Input;

    public abstract class ModalViewModelBase : ViewModelBase, IModalViewModel
    {
        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ModalViewModelBase(IObservable<bool>? okCanExecute = null)
        {

            var ok = okCanExecute is null
                ? ReactiveCommand.Create(OnOk, null, RxApp.MainThreadScheduler)
                : ReactiveCommand.Create(OnOk, okCanExecute, RxApp.MainThreadScheduler);
            var cancel = ReactiveCommand.Create(OnCancel);

            _disposables.Add(ok);
            _disposables.Add(cancel);

            OkCommand = ok;
            CancelCommand = cancel;
        }

        public void Cancel() => OnCancel();

        protected abstract void OnOk();

        protected abstract void OnCancel();
    }
}
