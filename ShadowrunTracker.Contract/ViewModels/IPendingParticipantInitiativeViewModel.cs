using ShadowrunTracker.Model;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IPendingParticipantInitiativeViewModel : IViewModel
    {
        ICharacterViewModel Character { get; }

        bool Blitz { get; set; }

        int? Roll { get; set; }

        bool SiezeInitiative { get; set; }

        InitiativeRoll? InitiativeRoll { get; }

        ICommand RollInitiativeCommand { get; }
    }
}
