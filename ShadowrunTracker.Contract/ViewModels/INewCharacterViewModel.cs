using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface INewCharacterViewModel : IReusableModalViewModel<ICharacterViewModel?>
    {
        ICharacterViewModel? Character { get; }

        IReadOnlyCollection<IInitiativeScoreViewModel> Initiatives { get; }
    }
}
