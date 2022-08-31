namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System.Collections.ObjectModel;

    public interface IParticipantInitiativeViewModel : IViewModel, IRecordViewModel<ParticipantInitiative>
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
