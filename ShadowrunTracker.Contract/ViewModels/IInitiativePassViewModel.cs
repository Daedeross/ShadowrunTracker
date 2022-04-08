using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IInitiativePassViewModel : IViewModel 
    {
        /// <summary>
        /// All participants in this initiative pass.
        /// </summary>
        ObservableCollection<IParticipantInitiativeViewModel> Participants { get; }

        /// <summary>
        /// The participant that is currently acting
        /// </summary>
        IParticipantInitiativeViewModel? ActiveParticipant { get; }

        ICancelable? RightFlyoutContext { get; }


        /// <summary>
        /// Move the initiative to the next participant
        /// </summary>
        void Next();

        /// <summary>
        /// Fired when the initiative pass is complete.
        /// </summary>
        event EventHandler<EventArgs> PassCompleted;

        ICommand QueryDamageCommand { get; }

        ICommand DelayActionCommand { get; }

        ICommand NextCommand { get; }
    }
}
