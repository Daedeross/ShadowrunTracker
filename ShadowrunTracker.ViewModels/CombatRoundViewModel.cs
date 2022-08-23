namespace ShadowrunTracker.ViewModels
{
    using DynamicData;
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.Utils;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public class CombatRoundViewModel : CanRequestConfirmationBase, ICombatRoundViewModel, IDisposable
    {
        private const string EndPassConfirmMessage = "Some participants have yet to act. Continue?";
        private const string EndRoundConfirmMessage = "Some participants still have a positive Initiative. Continue to next round?";
        private const int ActionCost = 10;

        private readonly IViewModelFactory _viewModelFactory;
        private readonly IDataStore<Guid> _store;

        public CombatRoundViewModel(IViewModelFactory viewModelFactory,
            IDataStore<Guid> store,
            IEnumerable<IParticipantInitiativeViewModel> participants,
            Guid? id = null)
        {
            _viewModelFactory = viewModelFactory;
            _store = store;

            Id = id ?? Guid.NewGuid();

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
            m_CurrentPass.Index = 0;
            InitiativePasses.Add(CurrentPass);
        }

        public Guid Id { get; }

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
                var newPass = new InitiativePassViewModel(_store, Participants);
                newPass.Index = InitiativePasses.Count;
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

        public CombatRound ToRecord()
        {
            return new CombatRound
            {
                Id = Id,
                Participants = Participants.Select(p => p.ToRecord()).ToList(),
                InitiativePasses = InitiativePasses.Select(ip => ip.ToRecord()).ToList()
            };
        }

        public void Update(CombatRound record)
        {
            if (record.Id != Id)
            {
                throw new ArgumentException($"Record id does not match: ViewModel Id: {Id} | Record Id: {record.Id}", nameof(record));
            }

            UpdateParticipants(record.Participants);
            UpdatePasses(record.InitiativePasses);

            if (record.CurrentPassIndex < 0 || record.CurrentPassIndex >= InitiativePasses.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(record.CurrentPassIndex));
            }
            else
            {
                CurrentPass = InitiativePasses[record.CurrentPassIndex];
            }
        }

        #region Update Helpers

        private void UpdateParticipants(IEnumerable<ParticipantInitiative> incomming)
        {
            var oldIds = Participants.Select(p => (p.Id, CharacterId: p.Character.Id)).ToList();
            var newIds = incomming.Select(p => (p.Id, p.CharacterId)).ToList();

            var removed = oldIds.Except(newIds);
            var added = newIds.Except(oldIds)
                .Join(incomming, tuple => tuple.Id, record => record.Id, (tuple, record) => record);

            RemoveParticipantsById(removed.Select(tuple => tuple.CharacterId));
            AddParticipantsFromRecords(added);
        }

        private void RemoveParticipantsById(IEnumerable<Guid> ids)
        {
            if (ids.Any())
            {
                foreach (var id in ids)
                {
                    var vm = _store.TryGet<ICharacterViewModel>(id);
                    if (vm.HasValue)
                    {
                        RemoveParticipant(vm.Value);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Id {id} not found in store");
                    }
                }
            }
        }

        private void AddParticipantsFromRecords(IEnumerable<ParticipantInitiative> records)
        {
            if (records.Any())
            {
                foreach (var participant in records)
                {
                    var vm = _store.TryGet<IParticipantInitiativeViewModel>(participant.Id);
                    if (vm.HasValue)
                    {
                        Participants.Add(vm.Value);
                        vm.Value.Update(participant);
                    }
                    else
                    {
                        var charVm = _store.TryGet<ICharacterViewModel>(participant.CharacterId);
                        if (charVm.HasValue)
                        {
                            AddParticipant(new ParticipantInitiativeViewModel(_store, charVm.Value, participant), true, participant.Acted);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Id {participant.Id} not found in store");
                        }
                    }
                }
            }
        }

        private void UpdatePasses(IEnumerable<InitiativePass> incomming)
        {
            var oldIds = InitiativePasses.Select(p => p.Id).ToList();
            var newIds = incomming.Select(p => p.Id).ToList();

            var removed = oldIds.Except(newIds);
            var added = newIds.Except(oldIds)
                .Join(incomming, id => id, record => record.Id, (id, record) => record);

            if (removed.Any())
            {
                throw new NotSupportedException("Unable to remove InitiativePasses");
            }

            AddPassesFromRecords(added);
            InitiativePasses.SortBy(p => p.Index);

            var joined = InitiativePasses.Join(incomming, vm => vm.Id, record => record.Id, (vm, record) => (vm, record)).ToList();
            if (joined.Count != InitiativePasses.Count)
            {
                throw new InvalidOperationException("Mismatched collection size.");
            }

            foreach (var (vm, record) in joined)
            {
                vm.Update(record);
            }
        }

        private void AddPassesFromRecords(IEnumerable<InitiativePass> added)
        {
            if (added.Any())
            {
                foreach (var record in added)
                {
                    var vm = new InitiativePassViewModel(_store, Participants, record.Id);
                    vm.Index = record.Index;
                    vm.Update(record);
                    InitiativePasses.Add(vm);
                }
            }
        }

        #endregion

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
