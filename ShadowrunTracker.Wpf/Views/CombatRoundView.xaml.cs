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
    /// Interaction logic for CombatRoundView.xaml
    /// </summary>
    public partial class CombatRoundView : ReactiveUserControl<ICombatRoundViewModel>
    {
        public CombatRoundView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.CurrentPass, v => v.CurrentPassHost.ViewModel)
                    .DisposeWith(d);
            });
        }
    }
}
