using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface ISelectableViewModel<T>
    {
        T? ViewModel { get; set; }

        bool IsSelected { get; set; }
    }
}
