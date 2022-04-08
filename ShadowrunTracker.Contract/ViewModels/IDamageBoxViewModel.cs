using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface IDamageBoxViewModel : IViewModel, IHoverable
    {
        bool IsFilled { get; }
        bool ShouldHighlight { get; set; }
    }
}
