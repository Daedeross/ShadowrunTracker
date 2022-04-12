using ReactiveUI;
using ShadowrunTracker.Data;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IEncounterViewModel : IViewModel
    {
        ObservableCollection<ICharacterViewModel> Participants { get; }

        ObservableCollection<ICombatRoundViewModel> Rounds { get; }

        ICombatRoundViewModel? CurrentRound { get; set; }

        void AddParticipant(ICharacterViewModel character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false);

        void AddParticipant(ICharacter character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false);

        ICommand NextRoundCommand { get; }

        ICommand NewParticipantCommand { get; }

        Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>> RequestInitiatives { get; }

        Interaction<ImportMode, ICharacterViewModel> GetNewCharacter { get; }
    }
}
