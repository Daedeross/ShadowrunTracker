#nullable disable

using ReactiveUI;
using ShadowrunTracker.ViewModels;
using ShadowrunTracker.Wpf.Helpers;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace ShadowrunTracker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for NewParticipantView.xaml
    /// </summary>
    public partial class NewParticipantView : ReactiveUserControl<INewCharacterViewModel>
    {
        public NewParticipantView()
        {
            InitializeComponent();

            DataContext = ViewModel;

            this.WhenActivated(d =>
            {
                DataContext = ViewModel;

                this.Bind(ViewModel, vm => vm.BaseEdge, v => v.BaseEdge.Value,
                                    i => (double)i, dbl => (int)(dbl ?? 1))
                                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.IsPlayer, v => v.IsPlayerCheck.IsChecked)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.IsPlayer, v => v.PlayerPanel.IsEnabled)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Player, v => v.PlayerName.Text)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.PainResistence, v => v.PainResistenceCombo.SelectedItem)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PainEditor, v => v.PainEditor.IsChecked)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.BaseBody, v => v.BaseBody.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseAgility, v => v.BaseAgility.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseReaction, v => v.BaseReaction.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseStrength, v => v.BaseStrength.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseCharisma, v => v.BaseCharisma.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseIntuition, v => v.BaseIntuition.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseLogic, v => v.BaseLogic.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseWillpower, v => v.BaseWillpower.Value,
                    x => x, x => (int) (x ?? 1))
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Initiatives, v => v.InitiativesList.ItemsSource)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.OkCommand, v => v.okButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.cancelButton)
                    .DisposeWith(d);
            });
        }

        private void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EventHelpers.OnNumericKeyboardFocus(sender, e);
        }
    }
}
