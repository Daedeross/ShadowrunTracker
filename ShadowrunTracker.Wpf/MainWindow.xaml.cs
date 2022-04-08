#nullable disable

using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

namespace ShadowrunTracker.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IViewFor<IWorkspaceViewModel>
    {
        private readonly IViewModelFactory _viewModelFactory;

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

        public MainWindow(IWorkspaceViewModel viewModel, IViewModelFactory viewModelFactory)
        {
            InitializeComponent();

            ViewModel = viewModel;
            _viewModelFactory = viewModelFactory;

            //requestInitiatives.DataContext = new Mock.MockRequestInitiativesViewModel();
                
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.NewEncounter, v => v.NewEncounterButton)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.CurrentEncounter, c => c.EncounterHost.ViewModel)
                    .DisposeWith(d);

                Interactions.ConfirmationRequest
                    .RegisterHandler(RequestConfirmation)
                    .DisposeWith(d);
            });

            DataContext = ViewModel;
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
    }
}
