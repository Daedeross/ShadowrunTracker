using ShadowrunTracker.Model;
using System;
using System.Collections.ObjectModel;

namespace ShadowrunTracker.ViewModels
{
    public interface IParticipantInitiativeViewModel : IViewModel
    {
        ICharacterViewModel Character { get; }
        int InitiativeScore { get; set; }
        bool SeizedInitiative { get; set; }
        bool Acted { get; set; }
        int TieBreaker { get; }
        bool CanAct { get; }

        ObservableCollection<IDamageBoxViewModel> PhysicalBoxes { get; }
        ObservableCollection<IDamageBoxViewModel> StunBoxes { get; }

        InitiativeRoll GetNextPass();
    }
}
