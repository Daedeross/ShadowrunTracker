namespace ShadowrunTracker.ViewModels
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Subjects;

    public abstract class DisposableModalViewModelBase<T> : ModalViewModelBase, IDisposableModalViewModel<T>
    {
        protected Subject<T> m_Complete;
        public IObservable<T> Complete => m_Complete;

        public DisposableModalViewModelBase(IObservable<bool>? okCanExecute = null)
            : base(okCanExecute)
        {
            m_Complete = new Subject<T>()
                .DisposeWith(_disposables);
        }
    }
}
