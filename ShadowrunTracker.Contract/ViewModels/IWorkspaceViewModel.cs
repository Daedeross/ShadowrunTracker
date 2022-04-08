using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public interface IWorkspaceViewModel : IViewModel
    {
        IEncounterViewModel? CurrentEncounter { get; }

        ICommand NewEncounter { get; }
    }
}
