using System.Windows.Controls;
using System.Windows.Input;

namespace ShadowrunTracker.Wpf.Helpers
{
    internal static class EventHelpers
    {
        public static void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            switch (sender)
            {
                case MahApps.Metro.Controls.NumericUpDown numeric:
                    numeric.SelectAll();
                    break;
                case TextBox text:
                    text.SelectAll();
                    break;
                default:
                    break;
            }
        }
    }
}
