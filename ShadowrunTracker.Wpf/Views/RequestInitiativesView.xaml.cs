#nullable disable
namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System.Reactive.Disposables;

    /// <summary>
    /// Interaction logic for RequestInitiativesView.xaml
    /// </summary>
    public partial class RequestInitiativesView : ReactiveUserControl<IRequestInitiativesViewModel>
    {
        public RequestInitiativesView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Participants, v => v.ParticipantsList.ItemsSource)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.RollAll, v => v.RollAllButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.OkCommand, v => v.OkButton)
                    .DisposeWith(d);

                DataContext = ViewModel;
            });

            DataContext = ViewModel;
        }
    }
}
