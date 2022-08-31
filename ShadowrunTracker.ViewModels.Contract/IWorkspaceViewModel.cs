namespace ShadowrunTracker.ViewModels
{
    using System.Windows.Input;

    /// <summary>
    /// Interface for the scope of the program.
    /// </summary>
    public interface IWorkspaceViewModel : IViewModel
    {
        /// <summary>
        /// The name of the session, if any.
        /// </summary>
        string? Name { get; set; }

        /// <summary>
        /// The current encounter.
        /// </summary>
        IEncounterViewModel? CurrentEncounter { get; }

        /// <summary>
        /// Start a new encounter.
        /// </summary>
        ICommand NewEncounter { get; }

        /// <summary>
        /// Try and connect to the communication server and start a session.
        /// </summary>
        ICommand StartSession { get; }

        /// <summary>
        /// Connect to a current session.
        /// </summary>
        ICommand ConnectToSession { get; }

        /// <summary>
        /// <see cref="IObservable{T}"/> that emits a value when a player encounter view is needed.
        /// </summary>
        IObservable<IEncounterViewModel> CurrentPlayerEncounter { get; }
    }
}
