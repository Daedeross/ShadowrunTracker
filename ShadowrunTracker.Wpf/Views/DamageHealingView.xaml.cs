using ShadowrunTracker.Wpf.Helpers;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShadowrunTracker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for DamageHealingView.xaml
    /// </summary>
    public partial class DamageHealingView : UserControl
    {
        public DamageHealingView()
        {
            InitializeComponent();
        }

        private void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EventHelpers.OnNumericKeyboardFocus(sender, e);
        }
    }
}
