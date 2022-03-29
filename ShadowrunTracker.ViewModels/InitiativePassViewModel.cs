using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public class InitiativePassViewModel : ReactiveObject, IInitiativePassViewModel
    {
        public InitiativePassViewModel()
        {
            Participants = new ObservableCollection<IParticipantInitiativeViewModel>();

            Acted = new ObservableCollection<IParticipantInitiativeViewModel>();
            NotActed = new ObservableCollection<IParticipantInitiativeViewModel>();
            NotActing = new ObservableCollection<IParticipantInitiativeViewModel>();
        }

        public ObservableCollection<IParticipantInitiativeViewModel> Participants { get; }

        public ObservableCollection<IParticipantInitiativeViewModel> Acted { get; }

        public ObservableCollection<IParticipantInitiativeViewModel> NotActed { get; }

        public ObservableCollection<IParticipantInitiativeViewModel> NotActing { get; }


        private IParticipantInitiativeViewModel? m_ActiveParticipant;
        public IParticipantInitiativeViewModel ActiveParticipant
        {
            get => m_ActiveParticipant;
            set => this.RaiseAndSetIfChanged(ref m_ActiveParticipant, value);
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs>? OnCompleted;
    }
}
