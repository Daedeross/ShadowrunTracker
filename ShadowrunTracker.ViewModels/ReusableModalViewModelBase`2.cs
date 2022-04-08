using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public abstract class ReusableModalViewModelBase<TInput, TOutput> : ViewModelBase, IReusableModalViewModel<TInput, TOutput>
    {
        protected Subject<TOutput>? _modalSubject;

        public ICommand OkCommand { get; protected set; }

        public ICommand CancelCommand { get; protected set; }
#nullable disable
        protected ReusableModalViewModelBase(bool deferOk = false, bool deferCancel = false)
        {
            if (!deferOk)
            {
                OkCommand = ReactiveCommand.Create(Ok, null, RxApp.MainThreadScheduler).DisposeWith(_disposables);
            }
            if (!deferCancel)
            {
                CancelCommand = ReactiveCommand.Create(DoCancel).DisposeWith(_disposables);
            }
        }
#nullable restore
        public ReusableModalViewModelBase(IObservable<bool>? okCanExecute = null)
        {
            var predicate = okCanExecute ?? OkCanExecute();
            OkCommand = ReactiveCommand.Create(Ok, predicate, RxApp.MainThreadScheduler).DisposeWith(_disposables);
            CancelCommand = ReactiveCommand.Create(DoCancel).DisposeWith(_disposables);
        }

        public IObservable<TOutput> Start(TInput input)
        {
            if (_modalSubject != null)
            {
                _disposables.Remove(_modalSubject);
                _modalSubject.Dispose();
            }
            
            _modalSubject = new Subject<TOutput>();
            OnStart(input);
            return _modalSubject;
        }

        public void Cancel() => CancelResult();

        protected virtual IObservable<bool>? OkCanExecute() => null;

        protected abstract void OnStart(TInput input);

        protected abstract TOutput OkResult();

        protected abstract TOutput CancelResult();

        protected abstract void OnReset();

        protected void Ok()
        {
            if (_modalSubject is null)
            {
                throw new InvalidOperationException("Modal is not active and should not Execute");
            }

            _modalSubject.OnNext(OkResult());
            _modalSubject.OnCompleted();
        }

        protected void DoCancel()
        {
            if (_modalSubject is null)
            {
                return;
            }
            _modalSubject.OnNext(CancelResult());
            _modalSubject.OnCompleted();
        }
    }
}
