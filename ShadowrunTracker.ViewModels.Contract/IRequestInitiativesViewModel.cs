namespace ShadowrunTracker.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;


    public interface IRequestInitiativesViewModel : IReusableModalViewModel<IEnumerable<ICharacterViewModel>, IEnumerable<IParticipantInitiativeViewModel>?>
    {
        ObservableCollection<IPendingParticipantInitiativeViewModel> Participants { get; }

        ICommand RollAll { get; }
    }
}
