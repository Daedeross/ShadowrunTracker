#nullable disable

namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using ShadowrunTracker.Wpf.Helpers;
    using System.Reactive.Disposables;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for NewCharacterEntryView.xaml
    /// </summary>
    public partial class NewCharacterEntryView : ReactiveUserControl<ICharacterViewModel>
    {
        public NewCharacterEntryView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Alias, v => v.Alias.Text)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.BaseEdge, v => v.BaseEdge.Value,
                    i => (double)i, dbl => (int)dbl)
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

                this.Bind(ViewModel, vm => vm.BaseBody, v => v.BaseBody.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseAgility, v => v.BaseAgility.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseReaction, v => v.BaseReaction.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseStrength, v => v.BaseStrength.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseCharisma, v => v.BaseCharisma.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseIntuition, v => v.BaseIntuition.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseLogic, v => v.BaseLogic.Value)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BaseWillpower, v => v.BaseWillpower.Value)
                    .DisposeWith(d);
            });
        }
        private void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EventHelpers.OnNumericKeyboardFocus(sender, e);
        }
    }
}
