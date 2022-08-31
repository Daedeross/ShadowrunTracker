#nullable disable

namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System.Reactive.Disposables;

    /// <summary>
    /// Interaction logic for ListSelectView.xaml
    /// </summary>
    public partial class ListSelectView : ReactiveUserControl<IListSelectViewModel>
    {
        public ListSelectView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                DataContext = ViewModel;

                this.Bind(ViewModel, vm => vm.Header, v => v.Header.Text)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.List, v => v.DataList.ItemsSource)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.SelectedItem, v => v.DataList.SelectedItem)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.OkCommand, v => v.okButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.cancelButton)
                    .DisposeWith(d);
            });

            DataContext = ViewModel;
        }
    }
}
