namespace ShadowrunTracker.ViewModels
{
    using DynamicData.Binding;
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System.Windows.Input;

    public interface IGmEncounterViewModel : IEncounterViewModel
    {
        IObservableCollection<ICharacterViewModel> Participants { get; }

        IObservableCollection<ICombatRoundViewModel> Rounds { get; }

        void AddParticipant(ICharacterViewModel character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false);

        void AddParticipant(Character character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false);

        ICommand NextRoundCommand { get; }

        ICommand NewParticipantCommand { get; }

        Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>> RequestInitiatives { get; }

        Interaction<ImportMode, ICharacterViewModel> GetNewCharacter { get; }
    }
}
