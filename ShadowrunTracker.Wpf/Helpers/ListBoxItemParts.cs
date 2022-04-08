using System;

namespace ShadowrunTracker.Wpf.Helpers
{

    [Flags]
    public enum ListBoxItemParts
    {
        None = 0,
        Selection = 1,
        MouseOver = 2,
        Ripple = 4,
        All = Selection | MouseOver | Ripple,
    }
}
