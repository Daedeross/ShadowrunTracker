using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IModalViewModel<TResult> : IViewModel, ICancelable, IDisposable
    {
        IObservable<TResult> Complete { get; }

        ICommand OkCommand { get; }

        ICommand CancelCommand { get; }
    }
}
