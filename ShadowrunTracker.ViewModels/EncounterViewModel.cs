using ReactiveUI;
using ShadowrunTracker.Data;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public class EncounterViewModel : ViewModelBase, IEncounterViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly SerialDisposable _requestSubscription;
        private readonly SerialDisposable _newParticipantSubscription;

        public EncounterViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));

            NextRoundCommand = ReactiveCommand.Create(() => NextRound());
            NewParticipantCommand = ReactiveCommand.Create(NewParticipant);

            RequestInitiatives = new Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>>();
            GetNewCharacter = new Interaction<Unit, ICharacterViewModel>();

            _requestSubscription = new SerialDisposable();
            _disposables.Add(_requestSubscription);

            _newParticipantSubscription = new SerialDisposable();
            _disposables.Add(_newParticipantSubscription);
        }

        public ObservableCollection<ICharacterViewModel> Participants { get; } = new ObservableCollection<ICharacterViewModel>();

        public ObservableCollection<ICombatRoundViewModel> Rounds { get; } = new ObservableCollection<ICombatRoundViewModel>();

        public Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>> RequestInitiatives { get; }

        public Interaction<Unit, ICharacterViewModel> GetNewCharacter { get; }

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
            }
        }

        public void AddParticipant(ICharacter character, InitiativeRoll? initiative = null, bool addToPass = false, bool acted = false)
        {
            AddParticipant(_viewModelFactory.Create(character), initiative, addToPass, acted);
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

        public ICommand NewParticipantCommand { get; }

        private void NewParticipant()
        {
            _newParticipantSubscription.Disposable = GetNewCharacter
                .Handle(Unit.Default)
                .Subscribe(p => AddParticipant(p));
        }
    }
}
