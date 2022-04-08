using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public abstract class ModalViewModelBase<T> : ViewModelBase, IModalViewModel<T>
    {
        protected Subject<T> m_Complete;
        public IObservable<T> Complete => m_Complete;

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ModalViewModelBase(IObservable<bool>? okCanExecute = null)
        {
            m_Complete = new Subject<T>();

            var ok = okCanExecute is null
                ? ReactiveCommand.Create(OnOk, null, RxApp.MainThreadScheduler)
                : ReactiveCommand.Create(OnOk, okCanExecute, RxApp.MainThreadScheduler);
            var cancel = ReactiveCommand.Create(OnCancel);

            _disposables.Add(m_Complete);
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
