namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Windows.Input;

    public class GmEncounterViewModel : EncounterViewModelBase, IGmEncounterViewModel
    {
        private readonly SerialDisposable _requestSubscription;
        private readonly SerialDisposable _newParticipantSubscription;

        public GmEncounterViewModel(IViewModelFactory viewModelFactory, IDataStore<Guid> store, Guid? id = null)
            : base(viewModelFactory, store, id)
        {
            _pushUpdate = true;
            NextRoundCommand = ReactiveCommand.Create(() => NextRound());
            NewParticipantCommand = ReactiveCommand.Create<ImportMode>(NewParticipant, outputScheduler: RxApp.MainThreadScheduler);

            RequestInitiatives = new Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>>();
            GetNewCharacter = new Interaction<ImportMode, ICharacterViewModel>();

            _requestSubscription = new SerialDisposable()
                .DisposeWith(_disposables);

            _newParticipantSubscription = new SerialDisposable()
                .DisposeWith(_disposables);

            Rounds.CollectionChanged += OnCollectionChanged;
            Participants.CollectionChanged += OnCollectionChanged;
            PropertyChanged += OnPropertyChanged;
        }

        public Interaction<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>> RequestInitiatives { get; }

        public Interaction<ImportMode, ICharacterViewModel> GetNewCharacter { get; }

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
            _pushUpdate = false;
            Rounds.Add(newRound);
            _pushUpdate = true;
            CurrentRound = newRound;
        }

        protected override void OnRoundComplete(object sender, EventArgs e)
        {
            #nullable disable
            CurrentRound.RoundComplete -= OnRoundComplete;
            #nullable enable
            NextRound();
        }

        public ICommand NewParticipantCommand { get; }

        private void NewParticipant(ImportMode mode)
        {
            _newParticipantSubscription.Disposable = GetNewCharacter
                .Handle(mode)
                .Subscribe(p => AddParticipant(p));
        }

        #region Change for Update push

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pushUpdate && e.PropertyName == nameof(CurrentRound))
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_pushUpdate)
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

        #endregion
    }
}
