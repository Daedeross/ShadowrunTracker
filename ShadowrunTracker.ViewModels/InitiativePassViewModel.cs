namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public class InitiativePassViewModel : ViewModelBase, IInitiativePassViewModel
    {
        private readonly IDataStore<Guid> _store;
        private readonly List<IParticipantInitiativeViewModel> _acted;
        private readonly List<IParticipantInitiativeViewModel> _notActed;
        private readonly List<IParticipantInitiativeViewModel> _notActing;

        public InitiativePassViewModel(IDataStore<Guid> store)
        {
            _store = store;

            Id = Guid.NewGuid();
            Participants = new ObservableCollection<IParticipantInitiativeViewModel>();

            _acted = new List<IParticipantInitiativeViewModel>();
            _notActed = new List<IParticipantInitiativeViewModel>();
            _notActing = new List<IParticipantInitiativeViewModel>();

            var queryDamage = ReactiveCommand.Create<ICharacterViewModel>(QueryDamageExecute);
            var delayAction = ReactiveCommand.Create<IParticipantInitiativeViewModel>(DelayActionExecute);
            var next = ReactiveCommand.Create(Next);

            _disposables.Add(queryDamage);
            _disposables.Add(delayAction);
            _disposables.Add(next);

            QueryDamageCommand = queryDamage;
            DelayActionCommand = delayAction;
            NextCommand = next;

            Participants.CollectionChanged += OnParcicipantsChanged;
        }

        public InitiativePassViewModel(IDataStore<Guid> store, IEnumerable<IParticipantInitiativeViewModel> participants, Guid? id = null)
        {
            _store = store;

            Id = id ?? Guid.NewGuid();
            var sorted = participants.ToList();
            sorted.Sort(ParticipantInitiativeReverseComparer.Default);

            Participants = new ObservableCollection<IParticipantInitiativeViewModel>(sorted);

            _acted = new List<IParticipantInitiativeViewModel>();
            _notActed = sorted.Where(p => p.InitiativeScore > 0).OrderBy(p => p, ParticipantInitiativeReverseComparer.Default).ToList();
            _notActing = sorted.Where(p => p.InitiativeScore <= 0).ToList();

            ActiveParticipant = _notActed.FirstOrDefault();

            var queryDamage = ReactiveCommand.Create<ICharacterViewModel>(QueryDamageExecute);
            var delayAction = ReactiveCommand.Create<IParticipantInitiativeViewModel>(DelayActionExecute);
            var next = ReactiveCommand.Create(Next);

            _disposables.Add(queryDamage);
            _disposables.Add(delayAction);
            _disposables.Add(next);

            QueryDamageCommand = queryDamage;
            DelayActionCommand = delayAction;
            NextCommand = next;

            Participants.CollectionChanged += OnParcicipantsChanged;
        }

        public Guid Id { get; }
        public int Index { get; set;  }

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

        public event EventHandler<EventArgs>? PassCompleted;

        private void OnParcicipantsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (IParticipantInitiativeViewModel item in e.OldItems)
                {
                    RemoveParticipant(item);
                }
            }
            if (e.NewItems != null)
            {
                foreach (IParticipantInitiativeViewModel item in e.NewItems)
                {
                    AddParticipant(item);
                }
            }
        }

        public void RemoveParticipant(IParticipantInitiativeViewModel participant)
        {
            participant.PropertyChanged -= OnParticipantPropertyChanged;
            _acted.Remove(participant);
            _notActed.Remove(participant);
            _notActing.Remove(participant);
        }

        public void AddParticipant(IParticipantInitiativeViewModel participant)
        {
            participant.PropertyChanged += OnParticipantPropertyChanged;
        }

        private void OnParticipantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is IParticipantInitiativeViewModel participant)
            {
                if (e.PropertyName == nameof(IParticipantInitiativeViewModel.InitiativeScore))
                {
                }
            }
        }

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
            if (record.Id != Id)
            {
                throw new ArgumentException($"Record id does not match: ViewModel Id: {Id} | Record Id: {record.Id}", nameof(record));
            }
            UpdateParticipants(record.ParticipantIds);

            ActiveParticipant = record.ActiveParticipantIndex >= 0
                ? Participants[record.ActiveParticipantIndex]
                : null;
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
            if (ids.Any())
            {
                foreach (var id in ids)
                {
                    var vm = _store.TryGet<IParticipantInitiativeViewModel>(id);
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

        private void AddParticipantsById(IEnumerable<Guid> ids)
        {
            if (ids.Any())
            {
                foreach (var id in ids)
                {
                    var vm = _store.TryGet<IParticipantInitiativeViewModel>(id);
                    if (vm.HasValue)
                    {
                        AddParticipant(vm.Value);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Id {id} not found in _store");
                    }
                }
            }
        }

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

        private void CleanupFlyoutContext()
        {
            RightFlyoutContext = null;
        }

        #endregion

        #region Next Command

        public ICommand NextCommand { get; }

        #endregion
    }
}
