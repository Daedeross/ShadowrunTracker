namespace ShadowrunTracker.ViewModels
{
    using System.Windows.Input;

    public interface IWorkspaceViewModel : IViewModel
    {
        IEncounterViewModel? CurrentEncounter { get; }

        ICommand NewEncounter { get; }
    }
}
