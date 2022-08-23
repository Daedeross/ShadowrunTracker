namespace ShadowrunTracker.ViewModels
{
    using DynamicData;
    using DynamicData.Binding;
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Windows.Input;

    public class EncounterViewModel : ViewModelBase, IEncounterViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IDataStore<Guid> _store;

        private readonly SerialDisposable _requestSubscription;
        private readonly SerialDisposable _newParticipantSubscription;

        /// <summary>
        /// For participants that are removed, but had at some point acted.
        /// For Encounter history/logs TBD.
        /// </summary>
        private readonly List<ICharacterViewModel> _removedParticipants = new List<ICharacterViewModel>();

        public EncounterViewModel(IViewModelFactory viewModelFactory, IDataStore<Guid> store, Guid? id = null)
        {
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
            _store = store;

            Id = id ?? Guid.NewGuid();

            NextRoundCommand = ReactiveCommand.Create(() => NextRound());
            NewParticipantCommand = ReactiveCommand.Create<ImportMode>(NewParticipant, outputScheduler: RxApp.MainThreadScheduler);

            RequestInitiatives = new Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>>();
            GetNewCharacter = new Interaction<ImportMode, ICharacterViewModel>();

            _requestSubscription = new SerialDisposable()
                .DisposeWith(_disposables);

            _newParticipantSubscription = new SerialDisposable()
                .DisposeWith(_disposables);
        }

        public IObservableCollection<ICharacterViewModel> Participants { get; } = new ObservableCollectionExtended<ICharacterViewModel>();

        public IObservableCollection<ICombatRoundViewModel> Rounds { get; } = new ObservableCollectionExtended<ICombatRoundViewModel>();

        public Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>> RequestInitiatives { get; }

        public Interaction<ImportMode, ICharacterViewModel> GetNewCharacter { get; }

        private ICombatRoundViewModel? m_CurrentRound;
        public ICombatRoundViewModel? CurrentRound
        {
            get => m_CurrentRound;
            set => this.RaiseAndSetIfChanged(ref m_CurrentRound, value);
        }

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
            AddParticipant(_viewModelFactory.Create(character), initiative, addToPass, acted);
        }

        public void RemoveParticipant(ICharacterViewModel character)
        {
            character.Remove -= OnRemoveCharacter;
            if (Participants.Remove(character))
            {
                if (CurrentRound != null)
                {
                    // If the character is in the current round, they should be archived.
                    if(CurrentRound.RemoveParticipant(character))
                    {
                        _removedParticipants.Add(character);
                    }
                }
            }
        }

        public ICommand NextRoundCommand { get; }

        public void NextRound()
        {
            _requestSubscription.Disposable = RequestInitiatives
                .Handle(Participants)
                .Subscribe(CreateNextRound)
                .DisposeWith(_disposables);
        }

        private void CreateNextRound(IEnumerable<IParticipantInitiativeViewModel> initiatives)
        {
            if (initiatives is null)
            {
                return;
            }
            var newRound = _viewModelFactory.CreateRound(initiatives);
            newRound.RoundComplete += OnRoundComplete;
            CurrentRound = newRound;
        }

        private void OnRoundComplete(object sender, EventArgs e)
        {
            #nullable disable
            CurrentRound.RoundComplete -= OnRoundComplete;
            #nullable enable
            NextRound();
        }

        private void OnRemoveCharacter(object sender, RemoveCharacterEventArgs e)
        {
            RemoveParticipant(e.Character);
        }

        public ICommand NewParticipantCommand { get; }

        public Guid Id { get; }

        private void NewParticipant(ImportMode mode)
        {
            _newParticipantSubscription.Disposable = GetNewCharacter
                .Handle(mode)
                .Subscribe(p => AddParticipant(p));
        }

        #region IRecordViewModel Implementation

        public Encounter ToRecord()
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
            UpdateParticipants(record.Participants);
        }
        private void UpdateParticipants(IEnumerable<Character> incomming)
        {
            var oldMap = Participants.ToDictionary(p => p.Id);
            var newMap = incomming.ToDictionary(p => p.Id);;

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
                        var charVm = _viewModelFactory.Create(participant);
                        AddParticipant(charVm);
                    }
                }
            }
        }

        #endregion
    }
}
