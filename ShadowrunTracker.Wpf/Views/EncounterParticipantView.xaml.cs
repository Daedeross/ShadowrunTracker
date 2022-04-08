#nullable disable

using ReactiveUI;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ShadowrunTracker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for RencounterParticipantView.xaml
    /// </summary>
    public partial class EncounterParticipantView : ReactiveUserControl<ICharacterViewModel>
    {
        public EncounterParticipantView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Alias, v => v.AliasText.Text)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.IsPlayer, v => v.PlayerText.Visibility)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Player, v => v.PlayerText.Text, x => $"({x})")
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Player, v => v.PlayerText.Text)
                    .DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.CurrentInitiative, vm => vm.CurrentInitiativeDice, (score, dice) => $"{score}+{dice}d6")
                    .BindTo(this, v => v.InitiativeText.Text)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.CurrentState, v => v.StateCombo.SelectedItem)
                    .DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.PhysicalDamage, vm => vm.PhysicalBoxes, (dam, max) => $"{max - dam} / {max}")
                    .BindTo(this, v => v.PhysicalDamageText.Text)
                    .DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.StunDamage, vm => vm.StunBoxes, (dam, max) => $"{max - dam} / {max}")
                    .BindTo(this, v => v.StunDamageText.Text)
                    .DisposeWith(d);
            });
        }
    }
}
