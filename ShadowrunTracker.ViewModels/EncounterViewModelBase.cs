namespace ShadowrunTracker.ViewModels
{
    using DynamicData.Binding;
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class EncounterViewModelBase : ViewModelBase, IEncounterViewModel
    {
        protected readonly IViewModelFactory _viewModelFactory;
        protected readonly IDataStore<Guid> _store;

        protected bool _pushUpdate;

        /// <summary>
        /// For participants that are removed, but had at some point acted.
        /// For Encounter history/logs TBD.
        /// </summary>
        private readonly List<ICharacterViewModel> _removedParticipants = new List<ICharacterViewModel>();

        public EncounterViewModelBase(IViewModelFactory viewModelFactory, IDataStore<Guid> store, Guid? id = null)
        {
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
            _store = store;

            Id = id ?? Guid.NewGuid();
        }

        public EncounterViewModelBase(IViewModelFactory viewModelFactory, IDataStore<Guid> store, Encounter record)
            :this(viewModelFactory, store, record.Id)
        {
            Update(record);
        }

        private ICombatRoundViewModel? m_CurrentRound;
        public ICombatRoundViewModel? CurrentRound
        {
            get => m_CurrentRound;
            set => this.RaiseAndSetIfChanged(ref m_CurrentRound, value);
        }

        public IObservableCollection<ICharacterViewModel> Participants { get; } = new ObservableCollectionExtended<ICharacterViewModel>();

        public IObservableCollection<ICombatRoundViewModel> Rounds { get; } = new ObservableCollectionExtended<ICombatRoundViewModel>();


        public void AddParticipant(ICharacterViewModel character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false)
        {
            if (character != null)
            {
                Participants.Add(character);
                if (initiative != null)
                {
                    CurrentRound?.AddParticipant(character, initiative, addToPass, acted);
                }
                character.Remove += OnRemoveCharacter;

                if (character is IDisposable disposable)
                {
                    _disposables.Add(disposable);
                }
            }
        }

        public void AddParticipant(Character character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false)
        {
            AddParticipant(_viewModelFactory.Create<ICharacterViewModel, Character>(character), initiative, addToPass, acted);
        }

        public void RemoveParticipant(ICharacterViewModel character)
        {
            character.Remove -= OnRemoveCharacter;
            if (Participants.Remove(character))
            {
                if (CurrentRound != null)
                {
                    // If the character is in the current round, they should be archived.
                    if (CurrentRound.RemoveParticipant(character))
                    {
                        _removedParticipants.Add(character);
                    }
                }
            }
        }

        protected void OnRemoveCharacter(object sender, RemoveCharacterEventArgs e)
        {
            RemoveParticipant(e.Character);
        }

        #region IRecordViewModel Implementation

        public Guid Id { get; }

        RecordBase IRecordViewModel.Record => ToRecord();

        public Encounter Record => ToRecord();

        public virtual Encounter ToRecord()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return new Encounter
            {
                Id = Id,
                Participants = Participants.Select(c => c.ToRecord()).ToList(),
                Rounds = Rounds.Select(r => r.ToRecord()).ToList(),
                CurrentRoundIndex = Rounds.IndexOf(CurrentRound)
            };
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public void Update(Encounter record)
        {
            try
            {
                _pushUpdate = false;

                UpdateParticipants(record.Participants);
                UpdateRounds(record.Rounds);
                CurrentRound = record.CurrentRoundIndex >= 0 && record.CurrentRoundIndex < Rounds.Count
                    ? Rounds[record.CurrentRoundIndex]
                    : null;
            }
            finally
            {
                _pushUpdate = true;
            }
        }
        private void UpdateParticipants(IEnumerable<Character> incomming)
        {
            var oldMap = Participants.ToDictionary(p => p.Id);
            var newMap = incomming.ToDictionary(p => p.Id); ;

            var removed = oldMap.Values.Where(vm => !newMap.ContainsKey(vm.Id));
            var added = newMap.Values.Where(record => !oldMap.ContainsKey(record.Id));
            var update = newMap.Join(oldMap, kvp => kvp.Key, kvp => kvp.Key, (inc, old) => (inc.Value, old.Value));

            RemoveParticipants(removed);

            foreach (var (record, viewModel) in update)
            {
                viewModel.Update(record);
            }

            AddParticipantsFromRecords(added);
        }

        private void RemoveParticipants(IEnumerable<ICharacterViewModel> vms)
        {
            if (vms.Any())
            {
                foreach (var vm in vms)
                {
                    RemoveParticipant(vm);
                }
            }
        }

        private void AddParticipantsFromRecords(IEnumerable<Character> records)
        {
            if (records.Any())
            {
                foreach (var participant in records)
                {
                    var vm = _store.TryGet<ICharacterViewModel>(participant.Id);
                    if (vm.HasValue)
                    {
                        AddParticipant(vm.Value);
                        vm.Value.Update(participant);
                    }
                    else
                    {
                        var charVm = _viewModelFactory.Create<ICharacterViewModel, Character>(participant);
                        AddParticipant(charVm);
                    }
                }
            }
        }

        private void UpdateRounds(IEnumerable<CombatRound> incomming)
        {
            var oldMap = Rounds.ToDictionary(p => p.Id);
            var newMap = incomming.ToDictionary(p => p.Id); ;

            var removed = oldMap.Values.Where(vm => !newMap.ContainsKey(vm.Id));
            var added = newMap.Values.Where(record => !oldMap.ContainsKey(record.Id));
            var update = newMap.Join(oldMap, kvp => kvp.Key, kvp => kvp.Key, (inc, old) => (inc.Value, old.Value));

            if (removed.Any())
            {
                throw new NotSupportedException("Cannot remove rounds.");
            }
            foreach (var (record, vm) in update)
            {
                vm.Update(record);
            }
            AddRoundsFromRecords(added);
        }

        private void AddRoundsFromRecords(IEnumerable<CombatRound> records)
        {
            foreach (var round in records)
            {
                ICombatRoundViewModel vm;
                var conditional = _store.TryGet<ICombatRoundViewModel>(round.Id);
                if (conditional.HasValue)
                {
                    vm = conditional.Value;
                }
                else
                {
                    vm = _viewModelFactory.CreateRound(new IParticipantInitiativeViewModel[0], round);
                }

                vm.Update(round);
                vm.RoundComplete += OnRoundComplete;
                Rounds.Add(vm);
            }
        }

        #endregion

        protected virtual void OnRoundComplete(object sender, EventArgs e)
        {
        }
    }
}
