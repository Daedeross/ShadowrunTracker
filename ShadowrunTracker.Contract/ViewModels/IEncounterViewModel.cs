using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ShadowrunTracker.Contract.ViewModels
{
    public interface IEncounterViewModel : IViewModel
    {
        ObservableCollection<ICharacterViewModel> Participants { get; }

        ObservableCollection<ICombatRoundViewModel> Rounds { get; }
    }
}
