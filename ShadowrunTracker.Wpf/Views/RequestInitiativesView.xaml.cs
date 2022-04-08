#nullable disable

using ReactiveUI;
using ShadowrunTracker.Model;
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
    /// Interaction logic for RequestInitiativesView.xaml
    /// </summary>
    public partial class RequestInitiativesView : ReactiveUserControl<IRequestInitiativesViewModel>
    {
        public RequestInitiativesView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
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
