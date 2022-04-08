using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface ICanRequestConfirmation
    {
        Interaction<string, bool> ConfirmationRequest { get; }
    }
}
