namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Communication;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.Utils;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class WorkspaceViewModel : ViewModelBase, IWorkspaceViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IDataStore<Guid> _store;
        private readonly IServiceFactory _serviceFactory;

        private ICommunicationService _webService;

        private string _username;
        private bool _isGm = true;

        public WorkspaceViewModel(IViewModelFactory viewModelFactory, IDataStore<Guid> store, IServiceFactory serviceFactory)
        {
            _viewModelFactory = viewModelFactory;
            _store = store;
            _serviceFactory = serviceFactory;

            _username = $"user_{Guid.NewGuid()}";
            _webService = _serviceFactory.CommunicationService(_username);

            var newEncounterCanExecute = this.WhenAnyValue(x => x.CurrentEncounter)
                .Select(e => e is null);

            NewEncounter = ReactiveCommand.Create(CreateNewEncounter, newEncounterCanExecute)
                .DisposeWith(_disposables);

            StartSession = ReactiveCommand.Create(EnterSessionName, newEncounterCanExecute)
                .DisposeWith(_disposables);

            ConnectToSession = ReactiveCommand.CreateFromTask(GetSessionNames, newEncounterCanExecute)
                .DisposeWith(_disposables);

            CurrentPlayerEncounter = this.WhenAnyValue(x => x.CurrentEncounter)
                .Select(e => e as IPlayerEncounterViewModel)
                .WhereNotNull();

            _store.CollectionChanged += StoreChanged;  // Uncomment for local debugging
        }

        private bool m_IsConnected;
        public bool IsConnected
        {
            get => m_IsConnected;
            set => this.RaiseAndSetIfChanged(ref m_IsConnected, value);
        }

        private IEncounterViewModel? m_CurrentEncounter;
        public IEncounterViewModel? CurrentEncounter
        {
            get => m_CurrentEncounter;
            set => this.RaiseAndSetIfChanged(ref m_CurrentEncounter, value);
        }

        public ICommand NewEncounter { get; }

        private string? m_Name = null;
        public string? Name
        {
            get => m_Name;
            set => this.RaiseAndSetIfChanged(ref m_Name, value);
        }

        public ICommand StartSession { get; }

        public ICommand ConnectToSession { get; }

        public IObservable<IEncounterViewModel> CurrentPlayerEncounter { get; }

        private void CreateNewEncounter()
        {
            var encounter = _viewModelFactory.Create<IGmEncounterViewModel>();
            if (encounter is IDisposable disposable)
            {
                _disposables.Add(disposable);
            }

            if (CurrentEncounter is IDisposable oldEncounter)
            {
                _disposables.Remove(oldEncounter);
                oldEncounter.Dispose();
            }

            CurrentEncounter = encounter;

            _webService.SyncRequest
                .SelectMany(async s => await OnSyncRequest(s))
                .Subscribe()
                .DisposeWith(_disposables);
        }

        private void EnterSessionName()
        {
            var options = new List<EntryDatum>
            {
                new EntryDatum("Session Name", typeof(string))
            };

#pragma warning disable CS8602 // Dereference of a possibly null reference. 
            Interactions.GetData
                .Handle(("Enter a name for the session", options))
                .SelectMany(async data => data is null ? null : await ConnectAsGMAsync(data[0].ToString(), default)) // analyzer does not recognize the null-check here...
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(s =>
                    {
                        if (s is null) return;

                        Name = s;
                        _isGm = true;
                        _store.CollectionChanged += StoreChanged;
                        IsConnected = true;
                        CreateNewEncounter();
                    })
                .DisposeWith(_disposables);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private async Task<string?> ConnectAsGMAsync(string? session, CancellationToken cancellationToken)
        {
            if (session is null)
            {
                return null;
            }

            await _webService.ConnectAsync();
            await _webService.ConnectToSession(session, true, cancellationToken);

            return session;
        }

        private async Task GetSessionNames(CancellationToken cancellationToken)
        {
            await _webService.ConnectAsync();
            var context = new SelectWithRefreshContext("Select the session to join", _webService.GetCurrentSessionsAsync);

            Interactions.SelectFromList
                .Handle(context)
                .SelectMany(async s => await ConnectAsPlayerAsync(s, default))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(s =>
                    {
                        if (s is null) return;

                        Name = s;
                        _isGm = false;
                        _store.CollectionChanged -= StoreChanged;
                        IsConnected = true;
                        _webService.RequestStateAsync(default);
                    })
                .DisposeWith(_disposables);
        }

        private async Task<string?> ConnectAsPlayerAsync(string? session, CancellationToken cancellationToken)
        {
            if (session is null)
            {
                return null;
            }

            await _webService.ConnectToSession(session, false, cancellationToken);
            _webService.Incomming.Subscribe(OnReceiveUpdate)
                .DisposeWith(_disposables);

            return session;
        }

        private async void StoreChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await OnStoreChangedAsync(sender, e);
        }

        internal async Task OnStoreChangedAsync(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isGm)
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item is INotifyPropertyChanged notify)
                        {
                            notify.PropertyChanged += EntityChanged;
                            if (item is IGmEncounterViewModel encounter)
                            {
                                await _webService.PushUpdateAsync(new Update { SessionName = Name, Record = encounter.Record });
                            }
                        }
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (item is INotifyPropertyChanged notify)
                        {
                            notify.PropertyChanged -= EntityChanged;
                        }
                    }
                } 
            }
        }

        private async void EntityChanged(object sender, PropertyChangedEventArgs e)
        {
            await OnEntityChangedAsync(sender, e);
        }

        internal async Task OnEntityChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IRecordViewModel recordViewModel && e.PropertyName == nameof(IRecordViewModel.Record))
            {
                await _webService.PushUpdateAsync(new Update { SessionName = Name, Record = recordViewModel.Record });
            }
        }

        private void OnReceiveUpdate(Update update)
        {
            if (update.Record is null)
            {
                return;
            }

            var vm = _store.SyncFromRecord(_viewModelFactory, update.Record);
            if (vm is IEncounterViewModel encounter)
            {
                CurrentEncounter = encounter;
            }
        }

        private async Task<Unit> OnSyncRequest(string player)
        {
            if (CurrentEncounter is null)
            {
                return Unit.Default;
            }
            await _webService.PushUpdateAsync(new Update { SessionName = Name, Record = CurrentEncounter.Record }, player);

            return Unit.Default;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _webService?.DisconnectAsync();
            }

            base.Dispose(disposing);
        }
    }
}
