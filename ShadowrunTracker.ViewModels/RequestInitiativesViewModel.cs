namespace ShadowrunTracker.ViewModels
{
    using DynamicData;
    using DynamicData.Binding;
    using ReactiveUI;
    using ShadowrunTracker.ViewModels.Internal;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public class RequestInitiativesViewModel : ReusableModalViewModelBase<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>?>, IRequestInitiativesViewModel
    {
        private ObservableCollection<IPendingParticipantInitiativeViewModel> m_Participants;
        private readonly IDataStore<Guid> _store;
        private readonly IObservableCollection<ICharacterViewModel> _characters;

        public ObservableCollection<IPendingParticipantInitiativeViewModel> Participants
        {
            get => m_Participants;
            set => this.RaiseAndSetIfChanged(ref m_Participants, value);
        }

        public ICommand RollAll { get; }

        public RequestInitiativesViewModel(IDataStore<Guid> store, IObservableCollection<ICharacterViewModel> characters)
            : base(false)
        {
            _store = store;

            m_Participants = new ObservableCollection<IPendingParticipantInitiativeViewModel>();
            _characters = characters;
            _characters.CollectionChanged += OnCharactersCollectionChanged;

            OkCommand = ReactiveCommand.Create(Ok, OkCanExecute())
                .DisposeWith(_disposables);

            RollAll = ReactiveCommand.Create(RollAllExecute)
                .DisposeWith(_disposables);
        }

        protected override IObservable<bool>? OkCanExecute()
        {
            return m_Participants
                .ToObservableChangeSet()
                .AutoRefresh(p => p.Roll)
                .ToCollection()
                .Select(x => x.All(p => p.Roll.HasValue));
        }

        protected override void OnStart(IEnumerable<ICharacterViewModel> input)
        {
            foreach (var p in Participants)
            {
                p.Roll = default;
            }
            //Participants = new ObservableCollection<IPendingParticipantInitiativeViewModel>(input.Select(c => new PendingParticipantInitiativeViewModel(c)));
        }

        protected override IEnumerable<IParticipantInitiativeViewModel> OkResult()
        {
            return Participants.Select(p => p.ToParticipant(_store));
        }

        protected override IEnumerable<IParticipantInitiativeViewModel>? CancelResult()
        {
            return default;
        }

        protected override void OnReset() { }

        private void RollAllExecute()
        {
            foreach (var participant in m_Participants)
            {
                participant.RollInitiativeCommand.Execute(null);
            }
        }

        private void OnCharactersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanging(nameof(Participants));
            if (e.NewItems != null)
            {
                foreach (ICharacterViewModel item in e.NewItems)
                {
                    Participants.Add(new PendingParticipantInitiativeViewModel(item));
                }
            }
            if (e.OldItems != null)
            {
                foreach (ICharacterViewModel item in e.OldItems)
                {
                    var find = Participants.Single(p => p.Character == item);
                    Participants.Remove(find);
                }
            }
            this.RaisePropertyChanged(nameof(Participants));
        }

        private bool _disposedValue;
        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _characters.CollectionChanged -= OnCharactersCollectionChanged;
                }

                _disposedValue = true;
            }
        }
    }
}
