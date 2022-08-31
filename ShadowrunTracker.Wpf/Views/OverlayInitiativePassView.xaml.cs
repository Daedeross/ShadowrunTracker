#nullable disable

namespace ShadowrunTracker.Wpf.Views
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for InitiativePassView.xaml
    /// </summary>
    [Name("Overlay")]
    public partial class OverlayInitiativePassView : ReactiveUserControl<IInitiativePassViewModel>
    {
        public OverlayInitiativePassView()
        {
            InitializeComponent();

            DataContext = ViewModel;

            this.WhenActivated(d =>
            {
                DataContext = ViewModel;
            });
        }
    }
}
