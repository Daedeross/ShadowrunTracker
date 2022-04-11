using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface ICanSave: IViewModel
    {
        ICommand SaveCommand { get; }

        public void Save();
    }
}
