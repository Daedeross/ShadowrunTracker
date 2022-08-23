namespace ShadowrunTracker
{
    using DynamicData.Binding;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.ViewModels;
    using System.Collections.Generic;

    public interface IViewModelFactory
    {
        T Create<T>() where T : class, IViewModel;

        ICharacterViewModel Create(Character loader);

        IParticipantInitiativeViewModel Create(ICharacterViewModel character, InitiativeRoll initiative);

        IRequestInitiativesViewModel Create(IObservableCollection<ICharacterViewModel> characters);

        ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel> participants);

        IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel> participants);

        public void Release(IViewModel viewModel);
    }
}
