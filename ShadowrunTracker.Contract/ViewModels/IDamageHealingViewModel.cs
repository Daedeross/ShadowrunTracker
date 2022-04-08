using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IDamageHealingViewModel : IModalViewModel<IDamageHealingViewModel>
    {
        ICharacterViewModel Character { get; }
        bool IsHealing { get; set; }
        int Physical { get; set; }
        int Stun { get; set; }
    }
}
