using ShadowrunTracker.Wpf.Helpers;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShadowrunTracker.Wpf.Views
{
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
