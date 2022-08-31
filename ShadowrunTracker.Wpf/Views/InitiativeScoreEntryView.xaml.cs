#nullable disable

namespace ShadowrunTracker.Wpf.Views
{
    using ShadowrunTracker.Wpf.Helpers;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for InitiativeScoreEntryView.xaml
    /// </summary>
    public partial class InitiativeScoreEntryView : UserControl
    {
        public InitiativeScoreEntryView()
        {
            InitializeComponent();
        }

        private void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EventHelpers.OnNumericKeyboardFocus(sender, e);
        }
    }
}
