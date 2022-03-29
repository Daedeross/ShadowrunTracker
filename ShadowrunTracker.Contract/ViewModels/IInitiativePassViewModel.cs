using System;
using System.Collections.ObjectModel;

namespace ShadowrunTracker.ViewModels
{
    public interface IInitiativePassViewModel : IViewModel 
    {
        /// <summary>
        /// All participants in this initiative pass.
        /// </summary>
        ObservableCollection<IParticipantInitiativeViewModel> Participants { get; }

        /// <summary>
        /// Participants that have already acted
        /// </summary>
        ObservableCollection<IParticipantInitiativeViewModel> Acted { get; }

        /// <summary>
        /// Participants that have not yet acted.
        /// </summary>
        ObservableCollection<IParticipantInitiativeViewModel> NotActed { get; }

        /// <summary>
        /// Participants that are not acting this pass.
        /// Likely because current initiative score < 1 or KO
        /// </summary>
        ObservableCollection<IParticipantInitiativeViewModel> NotActing { get; }

        /// <summary>
        /// The participant that is currently acting
        /// </summary>
        IParticipantInitiativeViewModel ActiveParticipant { get; }

        /// <summary>
        /// Move the initiative to the next participant
        /// </summary>
        void Next();

        /// <summary>
        /// Fired when the initiative pass is complete.
        /// </summary>
        event EventHandler<EventArgs> OnCompleted;
    }
}
