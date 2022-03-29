using System.Windows.Input;

namespace ShadowrunTracker.Wpf
{
    internal static class Helpers
    {
        public static void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var edit = (sender as MahApps.Metro.Controls.NumericUpDown);
            if (edit != null)
            {
                edit.SelectAll();
            }
        }
    }
}
