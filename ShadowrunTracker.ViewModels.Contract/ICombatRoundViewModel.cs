namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    public interface ICombatRoundViewModel : IViewModel, ICanRequestConfirmation, IRecordViewModel<CombatRound>
    {
        /// <summary>
        /// All participants in this combat round
        /// </summary>
        ObservableCollection<IParticipantInitiativeViewModel> Participants { get; }

        /// <summary>
        /// Each initiative pass that has been completed, or is in progress.
        /// </summary>
        ObservableCollection<IInitiativePassViewModel> InitiativePasses { get; }

        /// <summary>
        /// The current in itiative pass
        /// </summary>
        IInitiativePassViewModel? CurrentPass { get; }

        /// <summary>
        /// Move the initiative to the next participant
        /// </summary>
        ICommand NextToActCommad { get; }

        /// <summary>
        /// Goes to the next pass, regardless of if anyone has yet to act.
        /// </summary>
        ICommand EndPassCommand { get; }

        /// <summary>
        /// Ends the combat round.
        /// </summary>
        ICommand EndRoundCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> RoundComplete;

        void AddParticipant(IParticipantInitiativeViewModel participant, bool addToPass = false, bool acted = false);

        void AddParticipant(ICharacterViewModel character, InitiativeRoll roll, bool addToPass = false, bool acted = false);

        bool RemoveParticipant(IParticipantInitiativeViewModel participant);

        bool RemoveParticipant(ICharacterViewModel character);
    }
}
