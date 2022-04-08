using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IReusableModalViewModel<TOutput> : IViewModel, ICancelable, IDisposable
    {
        IObservable<TOutput> Start();

        ICommand OkCommand { get; }

        ICommand CancelCommand { get; }
    }
}
