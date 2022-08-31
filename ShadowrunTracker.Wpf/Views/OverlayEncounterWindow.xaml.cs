#nullable disable
namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System.Reactive.Disposables;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for OverlayEncounterWindow.xaml
    /// </summary>
    public partial class OverlayEncounterWindow : ReactiveWindow<IEncounterViewModel>
    {
        public OverlayEncounterWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                //this.OneWayBind(ViewModel, vm => vm.CurrentPass, v => v.CurrentPassHost.ViewModel)
                //    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.CurrentRound.CurrentPass, v => v.CurrentPassHost.ViewModel, pass =>
                {
                    _ = pass.Participants;
                    return pass;
                })
                    .DisposeWith(d);
            });
        }

        private void ReactiveWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
