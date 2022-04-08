using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IRequestInitiativesViewModel : IReusableModalViewModel<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>?>
    {
        ObservableCollection<IPendingParticipantInitiativeViewModel> Participants { get; }

        ICommand RollAll { get; }
    }
}
