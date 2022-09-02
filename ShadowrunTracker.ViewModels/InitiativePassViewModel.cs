namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.Utils;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public class InitiativePassViewModel : ViewModelBase, IInitiativePassViewModel
    {
        private static readonly ISet<string> _recordProperties = new HashSet<string>
        {
            nameof(ActiveParticipant)
        };

        private static readonly ISet<string> _participantWatchetProperties = new HashSet<string>
        {
            nameof(IParticipantInitiativeViewModel.SeizedInitiative),
            nameof(IParticipantInitiativeViewModel.InitiativeScore),
            nameof(IParticipantInitiativeViewModel.Acted),
            nameof(IParticipantInitiativeViewModel.CanAct),
        };

        private readonly IDataStore<Guid> _store;
        private List<IParticipantInitiativeViewModel> _acted;
        private List<IParticipantInitiativeViewModel> _notActed;
        private List<IParticipantInitiativeViewModel> _notActing;

        private bool _pushUpdate = true;

        public InitiativePassViewModel(IDataStore<Guid> store,
            IEnumerable<IParticipantInitiativeViewModel>? participants = null,
            InitiativePass? record = null)
        {
            _store = store;

            Id = record?.Id ?? Guid.NewGuid();
            var sorted = participants?.ToList() ?? new List<IParticipantInitiativeViewModel>();
            sorted.Sort(ParticipantInitiativeReverseComparer.Default);

            Participants = new ObservableCollection<IParticipantInitiativeViewModel>(sorted);

            _acted = new List<IParticipantInitiativeViewModel>();
            _notActed = sorted.Where(p => p.InitiativeScore > 0).OrderBy(p => p, ParticipantInitiativeReverseComparer.Default).ToList();
            _notActing = sorted.Where(p => p.InitiativeScore <= 0).ToList();

            ActiveParticipant = _notActed.FirstOrDefault();

            QueryDamageCommand = ReactiveCommand.Create<ICharacterViewModel>(QueryDamageExecute)
                .DisposeWith(_disposables);
            DelayActionCommand = ReactiveCommand.Create<IParticipantInitiativeViewModel>(DelayActionExecute)
                .DisposeWith(_disposables);
            NextCommand = ReactiveCommand.Create(Next)
                .DisposeWith(_disposables);

            Participants.CollectionChanged += OnParcicipantsChanged;

            if (record != null)
            {
                Update(record);
            }

            PropertyChanged += OnPropertyChanged;
        }

        #region Properties

        public Guid Id { get; }
        public int Index { get; set; }

        public ObservableCollection<IParticipantInitiativeViewModel> Participants { get; }

        private IParticipantInitiativeViewModel? m_ActiveParticipant;
        public IParticipantInitiativeViewModel? ActiveParticipant
        {
            get => m_ActiveParticipant;
            set => this.RaiseAndSetIfChanged(ref m_ActiveParticipant, value);
        }

        #region FlyoutContext

        private ICancelable? m_RightFlyoutContext;
        public ICancelable? RightFlyoutContext
        {
            get => m_RightFlyoutContext;
            set => this.RaiseAndSetIfChanged(ref m_RightFlyoutContext, value);
        }

        private IDamageHealingViewModel? DamageHealingContext => m_RightFlyoutContext as IDamageHealingViewModel;

        #endregion


        public event EventHandler<EventArgs>? PassCompleted;

        #endregion

        #region IRecordViewModel implementation

        RecordBase IRecordViewModel.Record => ToRecord();

        public InitiativePass Record => ToRecord();

        public InitiativePass ToRecord()
        {
            return new InitiativePass
            {
                Id = Id,
                Index = Index,
                ParticipantIds = Participants.Select(p => p.Id).ToList(),
                ActiveParticipantIndex = ActiveParticipant == null ? -1 : Participants.IndexOf(ActiveParticipant)
            };
        }

        public void Update(InitiativePass record)
        {
            try
            {
                _pushUpdate = false;

                if (record.Id != Id)
                {
                    throw new ArgumentException($"Record id does not match: ViewModel Id: {Id} | Record Id: {record.Id}", nameof(record));
                }
                UpdateParticipants(record.ParticipantIds);

                ActiveParticipant = record.ActiveParticipantIndex >= 0
                    ? Participants[record.ActiveParticipantIndex]
                    : null;
            }
            finally
            {
                _pushUpdate = true;
            }
        }

        private void UpdateParticipants(IEnumerable<Guid> incomming)
        {
            var oldIds = Participants.Select(p => p.Id).ToList();

            var removed = oldIds.Except(incomming);
            var added = incomming.Except(oldIds);

            RemoveParticipantsById(removed);
            AddParticipantsById(added);
        }

        private void RemoveParticipantsById(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                var vm = _store.TryGet<IParticipantInitiativeViewModel>(id);
                if (vm.HasValue)
                {
                    Participants.Remove(vm.Value);
                }
                else
                {
                    throw new InvalidOperationException($"Id {id} not found in store");
                }
            }
        }

        private void AddParticipantsById(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                var vm = _store.TryGet<IParticipantInitiativeViewModel>(id);
                if (vm.HasValue)
                {
                    Participants.Add(vm.Value);
                }
                else
                {
                    throw new InvalidOperationException($"Id {id} not found in _store");
                }
            }
        }

        private void Resort()
        {
            Participants.Sort(ParticipantInitiativeComparer.Default);

            _acted = new List<IParticipantInitiativeViewModel>();
            _notActed = Participants.Where(p => p.InitiativeScore > 0).OrderBy(p => p, ParticipantInitiativeReverseComparer.Default).ToList();
            _notActing = Participants.Where(p => p.InitiativeScore <= 0).ToList();

            ActiveParticipant = _notActed.FirstOrDefault();
        }

        #endregion

        #region Commands

        #region DamageHealing

        public ICommand QueryDamageCommand { get; }

        private void QueryDamageExecute(ICharacterViewModel character)
        {
            var subscription = Observable.Defer(() => ShowDamageHealingFlyout(character))
                .Subscribe(ApplyDamage, CleanupFlyoutContext);
            _disposables.Add(subscription);
        }

        private IObservable<IDamageHealingViewModel> ShowDamageHealingFlyout(ICharacterViewModel character)
        {
            var vm = new DamageHealingViewModel(character);
            RightFlyoutContext = vm;
            _disposables.Add(vm);
            return vm.Complete;
        }

        private void ApplyDamage(IDamageHealingViewModel result)
        {
            if (result.Physical != 0)
            {
                if (result.IsHealing)
                {
                    result.Character.ApplyPhysicalHealing(result.Physical);
                }
                else
                {
                    result.Character.ApplyPhysicalDamage(result.Physical);
                }
            }

            if (result.Stun != 0)
            {
                if (result.IsHealing)
                {
                    result.Character.ApplyStunHealing(result.Stun);
                }
                else
                {
                    result.Character.ApplyStunDamage(result.Stun);
                }
            }

            CleanupFlyoutContext();
        }

        #endregion

        #region DelayAction

        public ICommand DelayActionCommand { get; }

        private void DelayActionExecute(IParticipantInitiativeViewModel participant)
        {
            var subscription = Observable.Defer(() => ShowDelayActionFlyout(participant))
                .Subscribe(ApplyDelay, CleanupFlyoutContext);
            _disposables.Add(subscription);
        }

        private IObservable<IDelayActionViewModel> ShowDelayActionFlyout(IParticipantInitiativeViewModel participant)
        {
            var vm = new DelayActionViewModel(participant);
            RightFlyoutContext = vm;
            _disposables.Add(vm);
            return vm.Complete;
        }

        private void ApplyDelay(IDelayActionViewModel result)
        {
            if (result.CurrentAction != null)
            {
                result.Participant.InitiativeScore += result.CurrentAction.Value;
            }

            CleanupFlyoutContext();
        }

        #endregion

        #region Next Command

        public ICommand NextCommand { get; }

        public void Next()
        {
            if (ActiveParticipant != null)
            {
                ActiveParticipant.Acted = true;
                _notActed.Remove(ActiveParticipant);
                _acted.Add(ActiveParticipant);
            }

            if (_notActed.Any())
            {
                ActiveParticipant = _notActed.OrderBy(x => x, ParticipantInitiativeReverseComparer.Default).FirstOrDefault();
            }
            else
            {
                PassCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        private void CleanupFlyoutContext()
        {
            RightFlyoutContext = null;
        }

        #endregion

        #region Callbacks

        private void OnParcicipantsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool changed = false;
            if (e.OldItems != null)
            {
                foreach (IParticipantInitiativeViewModel item in e.OldItems)
                {
                    changed = true;
                    OnRemoveParticipant(item);
                }
            }
            if (e.NewItems != null)
            {
                foreach (IParticipantInitiativeViewModel item in e.NewItems)
                {
                    changed = true;
                    OnAddParticipant(item);
                }
            }
            if (_pushUpdate && changed)
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

        public void OnRemoveParticipant(IParticipantInitiativeViewModel participant)
        {
            participant.PropertyChanged -= OnParticipantPropertyChanged;
            _acted.Remove(participant);
            _notActed.Remove(participant);
            _notActing.Remove(participant);
        }

        public void OnAddParticipant(IParticipantInitiativeViewModel participant)
        {
            participant.PropertyChanged += OnParticipantPropertyChanged;
        }

        private void OnParticipantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is IParticipantInitiativeViewModel participant)
            {
                if (_participantWatchetProperties.Contains(e.PropertyName))
                {
                    Resort();
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pushUpdate && ReferenceEquals(this, sender) && _recordProperties.Contains(e.PropertyName))
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

        #endregion

        #region IDisposable

        private bool disposedValue;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PropertyChanged -= OnPropertyChanged;
                }

                disposedValue = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
