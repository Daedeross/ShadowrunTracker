#nullable disable
namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveMarbles.ObservableEvents;
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for OverlayEncounterWindow.xaml
    /// </summary>
    public partial class OverlayEncounterWindow : ReactiveWindow<IEncounterViewModel>
    {
        private const double ZoomFactor = 1d / 1200d;

        public OverlayEncounterWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                //this.OneWayBind(ViewModel, vm => vm.CurrentPass, v => v.CurrentPassHost.ViewModel)
                //    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.CurrentRound.CurrentPass, v => v.CurrentPassHost.ViewModel)
                    .DisposeWith(d);

                //this.Events()
                //    .PreviewMouseWheel
                //    .Do(Overlay_MouseWheel)
            });
        }

        private void ReactiveWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Overlay_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                DoZoom(e.Delta);
            }
        }

        private void DoZoom(int delta)
        {
            WindowScale.ScaleX += delta * ZoomFactor;
            WindowScale.ScaleY += delta * ZoomFactor;
        }
    }
}
