using ReactiveUI;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public class CombatRoundViewModel : CanRequestConfirmationBase, ICombatRoundViewModel, IDisposable
    {
        private const string EndPassConfirmMessage = "Some participants have yet to act. Continue?";
        private const string EndRoundConfirmMessage = "Some participants still have a positive Initiative. Continue to next round?";
        private const int ActionCost = 10;

        private readonly IViewModelFactory _viewModelFactory;

        public CombatRoundViewModel(IViewModelFactory viewModelFactory, IEnumerable<IParticipantInitiativeViewModel> participants)
        {
            _viewModelFactory = viewModelFactory;

            var nextToAct = ReactiveCommand.Create(NextToAct);
            var endPass = ReactiveCommand.Create(EndPass);
            var endRound = ReactiveCommand.Create(EndRound);

            NextToActCommad = nextToAct;
            EndPassCommand = endPass;
            EndRoundCommand = endRound;

            _disposables.Add(nextToAct);;
            _disposables.Add(endPass);
            _disposables.Add(endRound);

            InitiativePasses.CollectionChanged += this.OnInitiativePassesCollectionChanged;

            Participants = new ObservableCollection<IParticipantInitiativeViewModel>(participants);

            m_CurrentPass = _viewModelFactory.CreatePass(Participants);
            InitiativePasses.Add(CurrentPass);
        }


        public ObservableCollection<IParticipantInitiativeViewModel> Participants { get; }

        public ObservableCollection<IInitiativePassViewModel> InitiativePasses { get; } = new ObservableCollection<IInitiativePassViewModel>();

        private IInitiativePassViewModel m_CurrentPass;
        public IInitiativePassViewModel CurrentPass
        {
            get => m_CurrentPass;
            protected set => this.RaiseAndSetIfChanged(ref m_CurrentPass, value);
        }

        public ICommand NextToActCommad { get; }
        public ICommand EndPassCommand { get; }
        public ICommand EndRoundCommand { get; }

        public event EventHandler<EventArgs>? RoundComplete;

        private void EndRound()
        {
            if (CurrentPass.Participants.Any(p => (!p.Acted && p.InitiativeScore > 0)) == true)
            {
                RequestConfirmation(EndPassConfirmMessage, StartNextRound);
            }
            else if (Participants.Any(p => p.InitiativeScore > 0))
            {
                RequestConfirmation(EndRoundConfirmMessage, StartNextRound);
            }
            else
            {
                StartNextRound();
            }
        }

        public void EndPass()
        {
            if (CurrentPass.Participants.All(p => !p.Acted || p.InitiativeScore > 0) == true)
            {
                RequestConfirmation(EndPassConfirmMessage, StartNextPass);
            }
            else
            {
                StartNextPass();
            }
        }

        private void StartNextPass()
        {
            foreach (var particpiant in Participants)
            {
                particpiant.Acted = false;
                particpiant.InitiativeScore -= ActionCost;
            }

            if (Participants.Any(p => p.InitiativeScore > 0))
            {
                var newPass = new InitiativePassViewModel(Participants);
                InitiativePasses.Add(newPass);
                CurrentPass = newPass;
            }
            else
            {
                StartNextRound();
            }
        }

        private void OnPassComplete(object sender, EventArgs e)
        {
            StartNextPass();
        }

        private void StartNextRound()
        {
            RoundComplete?.Invoke(this, EventArgs.Empty);
        }

        public void NextToAct()
        {
            CurrentPass.Next();
        }

        private void OnInitiativePassesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (IInitiativePassViewModel newPass in e.NewItems)
                {
                    newPass.PassCompleted += OnPassComplete;
                }
            }
            if (e.OldItems != null)
            {
                foreach (IInitiativePassViewModel oldPass in e.OldItems)
                {
                    oldPass.PassCompleted -= OnPassComplete;
                }
            }
        }

        public void AddParticipant(IParticipantInitiativeViewModel participant, bool addToPass = false, bool acted = false)
        {
            Participants.Add(participant);
            if (addToPass)
            {
                participant.Acted = acted;
                CurrentPass.Participants.Add(participant);
            }
        }

        public void AddParticipant(ICharacterViewModel character, InitiativeRoll roll, bool addToPass = false, bool acted = false)
        {
            AddParticipant(_viewModelFactory.Create(character, roll));
        }

        public bool RemoveParticipant(IParticipantInitiativeViewModel participant)
        {
            if (Participants.Remove(participant))
            {
                CurrentPass.Participants.Remove(participant);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveParticipant(ICharacterViewModel character)
        {
            var participant = Participants.SingleOrDefault(p => Equals(p.Character, character));
            if (participant is null)
            {
                return false; ;
            }
            else
            {
                return RemoveParticipant(participant);
            }
        }

        #region IDisposable

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var pass in InitiativePasses)
                    {
                        pass.PassCompleted -= this.OnPassComplete;
                    }
                }

                _disposedValue = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
