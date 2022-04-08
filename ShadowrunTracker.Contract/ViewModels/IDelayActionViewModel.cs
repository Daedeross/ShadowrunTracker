using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IDelayActionViewModel : IModalViewModel<IDelayActionViewModel>
    {
        IParticipantInitiativeViewModel Participant { get; }

        IReadOnlyCollection<DelayAction> Actions { get; }

        DelayAction? CurrentAction { get; set; }
    }
}
