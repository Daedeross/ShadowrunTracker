using System.Collections.ObjectModel;

namespace ShadowrunTracker.Contract.ViewModels
{
    public interface ICombatRoundViewModel : IViewModel
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
        IInitiativePassViewModel CurrentPass { get; }

        /// <summary>
        /// Move the initiative to the next participant
        /// </summary>
        void NextToAct();

        /// <summary>
        /// Goes to the next pass, regardless of if anyone has yet to act.
        /// </summary>
        void NextPass();

        /// <summary>
        /// Ends the combat round.
        /// </summary>
        void EndRound();
    }
}
