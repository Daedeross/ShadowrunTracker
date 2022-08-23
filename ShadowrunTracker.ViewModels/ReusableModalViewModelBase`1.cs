namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using System;
    using System.Reactive.Subjects;
    using System.Windows.Input;

    public abstract class ReusableModalViewModelBase<T> : ViewModelBase, IReusableModalViewModel<T>
    {
        protected Subject<T>? _modalSubject;

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ReusableModalViewModelBase(IObservable<bool>? okCanExecute = null)
        {
            var ok = okCanExecute is null
                ? ReactiveCommand.Create(Ok, null, RxApp.MainThreadScheduler)
                : ReactiveCommand.Create(Ok, okCanExecute, RxApp.MainThreadScheduler);
            var cancel = ReactiveCommand.Create(DoCancel);

            _disposables.Add(ok);
            _disposables.Add(cancel);

            OkCommand = ok;
            CancelCommand = cancel;
        }

        public IObservable<T> Start()
        {
            if (_modalSubject != null)
            {
                _disposables.Remove(_modalSubject);
                _modalSubject.Dispose();
            }
            
            _modalSubject = new Subject<T>();
            OnStart();
            return _modalSubject;
        }

        public void Cancel() => CancelResult();

        protected abstract void OnStart();

        protected abstract T OkResult();

        protected abstract T CancelResult();

        protected abstract void OnReset();

        private void Ok()
        {
            if (_modalSubject is null)
            {
                throw new InvalidOperationException("Modal is not active and should not Execute");
            }

            _modalSubject.OnNext(OkResult());
            _modalSubject.OnCompleted();
            OnReset();
        }

        private void DoCancel()
        {
            if (_modalSubject is null)
            {
                return;
            }
            _modalSubject.OnNext(CancelResult());
            _modalSubject.OnCompleted();
            OnReset();
        }
    }
}
