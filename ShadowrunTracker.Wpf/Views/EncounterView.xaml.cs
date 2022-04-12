#nullable disable

using MaterialDesignThemes.Wpf;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using ShadowrunTracker.Utils;
using ShadowrunTracker.Model;
using ShadowrunTracker.Data;
using System.IO;
using Newtonsoft.Json;

namespace ShadowrunTracker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for EncounterView.xaml
    /// </summary>
    public partial class EncounterView : ReactiveUserControl<IEncounterViewModel>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ConcurrentDictionary<Dock, ViewModels.ICancelable> _dockContexts = new ConcurrentDictionary<Dock, ViewModels.ICancelable>();
        private INewCharacterViewModel _newCharacterViewModel;
        private IRequestInitiativesViewModel _requestInitiativesViewModel;

        public EncounterView(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;

            InitializeComponent();

            this.WhenActivated(
                d =>
                {
                    _newCharacterViewModel = _viewModelFactory.Create<INewCharacterViewModel>()
                        .DisposeWith(d);

                    BottomDrawerContent.ViewModel = _newCharacterViewModel;
                    BottomDrawerContent.DataContext = _newCharacterViewModel;
                    BottomDrawerContent.Events()
                        .Loaded
                        .Do(x => Debug.WriteLine("LOADED>>>"));

                    _requestInitiativesViewModel = new RequestInitiativesViewModel(ViewModel.Participants)
                        .DisposeWith(d);

                    LeftDrawer.ViewModel = _requestInitiativesViewModel;
                    LeftDrawer.DataContext = _requestInitiativesViewModel;

                    ViewModel.RequestInitiatives
                        .RegisterHandler(c => RequestInitiatives(c, d))
                        .DisposeWith(d);

                    this.BindInteraction(ViewModel, vm => vm.GetNewCharacter, GetNewCharacter)
                        .DisposeWith(d);

                    this.BindCommand(ViewModel,
                                     vm => vm.NewParticipantCommand,
                                     v => v.NewParticipantButton,
                                     Observable.Return(ImportMode.New))
                        .DisposeWith(d);

                    this.BindCommand(ViewModel,
                                     vm => vm.NewParticipantCommand,
                                     v => v.LoadParticipantButton,
                                     Observable.Return(ImportMode.File))
                        .DisposeWith(d);

                    this.BindCommand(ViewModel, vm => vm.NextRoundCommand, v => v.NewRoundButton)
                        .DisposeWith(d);

                    this.OneWayBind(ViewModel, vm => vm.CurrentRound, v => v.CurrentRoundHost.ViewModel)
                        .DisposeWith(d);

                    EncounterDrawerHost.Events()
                        .DrawerClosing
                        .Do(a => CancelDockModal(a.Dock))
                        .Subscribe(x => { })
                        .DisposeWith(d);

                    ViewModel.AddParticipant(Mock.TestData.TestCharacters.CreateViewModel("Bob", 12, 2));

                    this.OneWayBind(ViewModel, vm => vm.Participants, v => v.ParticipantsList.ItemsSource)
                        .DisposeWith(d);

                    DataContext = ViewModel;
                });

            DataContext = ViewModel;
        }

        private IObservable<Unit> RequestInitiatives(InteractionContext<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>> context,
                                                     CompositeDisposable disposables)
        {
            //DialogHost.Show(_requestInitiativesViewModel);//, "WindowDialogHost");
            _dockContexts.AddOrUpdate(Dock.Left, _requestInitiativesViewModel, (d, vm) => { vm.Cancel(); return _requestInitiativesViewModel; });
            EncounterDrawerHost.IsLeftDrawerOpen = true;

            return _requestInitiativesViewModel.Start(context.Input)
                .Do(initiatives =>
                {
                    _dockContexts.Remove(Dock.Left, out var _);
                    EncounterDrawerHost.IsLeftDrawerOpen = false;
                    //DialogHost.Close(null);
                    context.SetOutput(initiatives);
                })
                .Select(x => Unit.Default);
        }

        private IObservable<Unit> GetNewCharacter(InteractionContext<ImportMode, ICharacterViewModel> context)
        {
            return context.Input switch
            {
                ImportMode.New => NewCharacterEntry(context),
                //ImportMode.Library =>
                ImportMode.File => LoadCharacter(context),
                //ImportMode.ChummerFile =>
                _ => Observable.Empty<Unit>()
            };
        }

        private IObservable<Unit> NewCharacterEntry(InteractionContext<ImportMode, ICharacterViewModel> context)
        {
            _dockContexts.AddOrUpdate(Dock.Bottom, _newCharacterViewModel, (d, vm) =>
            {
                vm.Cancel();
                return _newCharacterViewModel;
            });

            EncounterDrawerHost.IsBottomDrawerOpen = true;

            return _newCharacterViewModel.Start()
                .Do(character =>
                {
                    _dockContexts.Remove(Dock.Bottom, out var _);
                    EncounterDrawerHost.IsBottomDrawerOpen = false;
                    context.SetOutput(character);
                }
                ).Select(x => Unit.Default);
        }

        private IObservable<Unit> LoadCharacter(InteractionContext<ImportMode, ICharacterViewModel> context)
        {
            return Observable.Start(LoadCharacterFromFile, RxApp.MainThreadScheduler)
                .Do(c => context.SetOutput(_viewModelFactory.Create(c)))
                .Select(x => Unit.Default);
        }

        private ICharacter LoadCharacterFromFile()
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    DefaultExt = ".json",
                    Filter = "Json documents (.json)|*.json"
                };

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    var ser = JsonSerializer.CreateDefault();
                    using (var stream = dialog.OpenFile())
                    {
                        var reader = new JsonTextReader(new StreamReader(stream));
                        var foo = ser.Deserialize<Character>(reader);
                        return foo;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);

                return null;
            }
        }

        private void CancelDockModal(Dock dock)
        {
            if (_dockContexts.TryRemove(dock, out var vm))
            {
                vm.Cancel();
            }
        }
    }
}
