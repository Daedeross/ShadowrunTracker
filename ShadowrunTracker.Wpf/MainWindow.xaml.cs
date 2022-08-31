#nullable disable

namespace ShadowrunTracker.Wpf
{
    using MahApps.Metro.Controls;
    using MaterialDesignThemes.Wpf;
    using ReactiveMarbles.ObservableEvents;
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.Utils;
    using ShadowrunTracker.ViewModels;
    using ShadowrunTracker.Wpf.Views;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Text.Json;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IViewFor<IWorkspaceViewModel>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private IDrawerManager _drawerManager;
        private ConcurrentDictionary<Type, Window> _childWindows = new ConcurrentDictionary<Type, Window>();

        public IWorkspaceViewModel ViewModel
        {
            get { return (IWorkspaceViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IWorkspaceViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IWorkspaceViewModel)value;
        }

        public MainWindow(IWorkspaceViewModel viewModel, IViewModelFactory viewModelFactory, IDrawerManager drawerManager)
        {
            InitializeComponent();

            ViewModel = viewModel;
            _viewModelFactory = viewModelFactory;
            _drawerManager = drawerManager;

            //requestInitiatives.DataContext = new Mock.MockRequestInitiativesViewModel();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.NewEncounter, v => v.NewEncounterButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.StartSession, v => v.StartSessionButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.ConnectToSession, v => v.ConnectToSessionButton)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.CurrentEncounter, c => c.EncounterHost.ViewModel, e => e as IGmEncounterViewModel)
                    .DisposeWith(d);

                ViewModel.CurrentPlayerEncounter
                    .WhereNotNull()
                    .Subscribe(e => ShowChildWindow(e, () => new OverlayEncounterWindow()))
                    .DisposeWith(d);

                BindDrawers(d);

                EncounterDrawerHost.Events()
                    .DrawerClosing
                    .Do(a => CancelDockModal(a.Dock))
                    .Subscribe(x => { })
                    .DisposeWith(d);

                Interactions.ConfirmationRequest
                    .RegisterHandler(RequestConfirmation)
                    .DisposeWith(d);

                Interactions.SaveDialog
                    .RegisterHandler(SaveObject)
                    .DisposeWith(d);

                Interactions.GetData
                    .RegisterHandler(ctx => ShowDataEntryDialog(ctx, d))
                    .DisposeWith(d);

                Interactions.SelectFromList
                    .RegisterHandler(ctx => ShowListSelectDialog(ctx, d))
                    .DisposeWith(d);
            });

            DataContext = ViewModel;
        }

        private void BindDrawers(CompositeDisposable d)
        {
            this.WhenAnyValue(v => v._drawerManager.LeftDrawerVisible)
                .Subscribe(b => EncounterDrawerHost.IsLeftDrawerOpen = b)
                .DisposeWith(d);
            this.WhenAnyValue(v => v._drawerManager.LeftDrawerContext)
                .Subscribe(vm => LeftDrawerContent.ViewModel = vm)
                .DisposeWith(d);

            //this.WhenAnyValue(v => v._drawerManager.RightDrawerVisible)
            //    .Subscribe(b => EncounterDrawerHost.IsRightDrawerOpen = b)
            //    .DisposeWith(d);
            //this.WhenAnyValue(v => v._drawerManager.RightDrawerContext)
            //    .Subscribe(vm => RightDrawerContent.ViewModel = vm)
            //    .DisposeWith(d);

            //this.WhenAnyValue(v => v._drawerManager.TopDrawerVisible)
            //    .Subscribe(b => EncounterDrawerHost.IsTopDrawerOpen = b)
            //    .DisposeWith(d);
            //this.WhenAnyValue(v => v._drawerManager.TopDrawerContext)
            //    .Subscribe(vm => TopDrawerContent.ViewModel = vm)
            //    .DisposeWith(d);

            this.WhenAnyValue(v => v._drawerManager.BottomDrawerVisible)
                .Subscribe(b => EncounterDrawerHost.IsBottomDrawerOpen = b)
                .DisposeWith(d);
            this.WhenAnyValue(v => v._drawerManager.BottomDrawerContext)
                .Subscribe(vm => BottomDrawerContent.ViewModel = vm)
                .DisposeWith(d);
        }

        private IObservable<Unit> RequestConfirmation(InteractionContext<string, bool> context)
        {
            DialogHost.Show(context.Input);

            return WindowDialogHost.Events()
                .DialogClosing
                .Do(e =>
                {
                    context.SetOutput((bool)e.Parameter);
                })
                .Select(x => Unit.Default);
        }

        private void DialogHost_DialogClosing(object sender, DialogClosingEventArgs e)
        {
            if (e.Parameter is ViewModels.ICancelable cancelable)
            {
                cancelable.Cancel();
            }
        }

        private void SaveObject(InteractionContext<SaveContext, string> context)
        {
            if (context.Input.SaveAs || context.Input.Filename is null)
            {
                var filename = context.Input.Filename ?? "character";
                context.SetOutput(SaveDialog(context.Input.Object, filename));
            }
            else
            {
                context.SetOutput(SaveToFile(context.Input.Object, context.Input.Filename));
            }
        }

        private string SaveDialog(object obj, string filename)
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = $"{filename}",
                    DefaultExt = ".json",
                    Filter = "Json documents (.json)|*.json" // Filter files by extension
                };

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    using (var stream = dialog.OpenFile())
                    {
                        JsonSerializer.Serialize(stream, obj);
                    }

                    return dialog.FileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return null;
            }
        }

        private string SaveToFile(object obj, string filename)
        {
            try
            {
                using var stream = new FileStream(filename, FileMode.Open);
                JsonSerializer.Serialize(stream, obj);

                return filename;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return null;
            }
        }

        private Character LoadCharacter()
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
                    using (var stream = dialog.OpenFile())
                    {
                        return JsonSerializer.Deserialize<Character>(stream);
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

        private IObservable<Unit> ShowDataEntryDialog(InteractionContext<(string Header, IList<EntryDatum> Options), IList<object>> context, CompositeDisposable disposables)
        {
            IDataEntryModalViewModel vm = new DataEntryModalViewModel(context.Input.Header, context.Input.Options);
            return ShowModalDialog<IDataEntryModalViewModel, IList<object>>(vm, context.SetOutput, disposables);
        }

        private IObservable<Unit> ShowListSelectDialog(InteractionContext<SelectWithRefreshContext, string> context, CompositeDisposable disposables)
        {
            var vm = new ListSelectViewModel(context.Input.Header, context.Input.Refresh);
            vm.RefreshAsync(default);
            return ShowModalDialog<IListSelectViewModel, string>(vm, context.SetOutput, disposables);
        }

        private IObservable<Unit> ShowModalDialog<TViewModel, TOut>(TViewModel viewModel, Action<TOut> onNext, CompositeDisposable disposables)
            where TViewModel : IDisposableModalViewModel<TOut>
        {
            WindowDialogHost.DialogContent = viewModel;
            WindowDialogHost.IsOpen = true;

            viewModel.Complete.Subscribe(onNext, () =>
            {
                WindowDialogHost.IsOpen = false;
                viewModel.Dispose();
            }).DisposeWith(disposables);


            return viewModel.Complete
                //.Do(onNext, () =>
                //{
                //    WindowDialogHost.IsOpen = false;
                //    viewModel.Dispose();
                //})
                .Select(x => Unit.Default);
        }

        private void CancelDockModal(Dock dock)
        {
            _drawerManager.CancelDrawer(dock);
        }

        private void ShowChildWindow<T>(T viewModel, Func<ReactiveWindow<T>> factory = null)
            where T : class, IViewModel
        {
            Func<Type, Window> _factory = factory is null
                ? t => new ReactiveWindow<T>()
                : t => factory();

            var window = _childWindows.AddOrUpdate(typeof(T), _factory, (t, w) => w);
            if (window is ReactiveWindow<T> child)
            {
                child.ViewModel = viewModel;
                child.Show();
                child.Activate();
                child.Closed += (s, e) => _childWindows.TryRemove(typeof(T), out var _);
                WindowState = WindowState.Minimized;
            }
            else
            {
                throw new InvalidCastException("Invalid window type.");
            }
        }
    }
}
