using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShadowrunTracker.Wpf.Helpers
{

    public static class MyListBoxItemAssist
    {
        #region ShowParts
        public static readonly DependencyProperty ShowPartsProperty = DependencyProperty.RegisterAttached("ShowParts", typeof(ListBoxItemParts), typeof(MyListBoxItemAssist), new PropertyMetadata(ListBoxItemParts.All));

        public static ListBoxItemParts GetShowParts(DependencyObject element)
        {
            return (ListBoxItemParts)element.GetValue(ShowPartsProperty);
        }

        public static void SetShowParts(DependencyObject element, ListBoxItemParts value)
        {
            element.SetValue(ShowPartsProperty, value);
        }
        #endregion

        public static readonly DependencyProperty HoverIndexProperty =
            DependencyProperty.RegisterAttached("HoverIndex", typeof(int), typeof(MyListBoxItemAssist), new PropertyMetadata(-1));


        public static int GetHoverIndex(DependencyObject element)
        {
            return (int)element.GetValue(HoverIndexProperty);
        }

        public static void SetHoverIndex(DependencyObject element, int value)
        {
            element.SetValue(HoverIndexProperty, value);
        }

        public static readonly DependencyProperty MyIndexProperty =
            DependencyProperty.RegisterAttached("MyIndex", typeof(int), typeof(MyListBoxItemAssist), new PropertyMetadata(-1));

        public static int GetMyIndex(DependencyObject element)
        {
            return (int)element.GetValue(MyIndexProperty);
        }

        public static void SetMyIndex(DependencyObject element, int value)
        {
            element.SetValue(MyIndexProperty, value);
        }

        #region RippleBrush

        public static readonly DependencyProperty RippleBrushProperty =
            DependencyProperty.RegisterAttached("RippleBrush", typeof(bool), typeof(MyListBoxItemAssist), new PropertyMetadata(false));

        public static bool GetRippleBrush(DependencyObject element)
        {
            return (bool)element.GetValue(RippleBrushProperty);
        }

        public static void SetRippleBrush(DependencyObject element, bool value)
        {
            element.SetValue(RippleBrushProperty, value);
        } 
        #endregion

    }
}
