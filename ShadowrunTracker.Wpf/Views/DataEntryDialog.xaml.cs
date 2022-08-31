#nullable disable
namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System.Reactive.Disposables;

    /// <summary>
    /// Interaction logic for DataEntryDialog.xaml
    /// </summary>
    public partial class DataEntryDialog : ReactiveUserControl<IDataEntryModalViewModel>
    {
        public DataEntryDialog()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Header, v => v.Header.Text)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Data, v => v.DataList.ItemsSource)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.OkCommand, v => v.okButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.cancelButton)
                    .DisposeWith(d);
            });
        }
    }
}
