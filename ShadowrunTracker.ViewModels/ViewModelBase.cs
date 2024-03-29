﻿namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using System;
    using System.Reactive.Disposables;

    public abstract class ViewModelBase : ReactiveObject, IViewModel, IDisposable
    {
        protected readonly CompositeDisposable _disposables = new();

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _disposables.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
