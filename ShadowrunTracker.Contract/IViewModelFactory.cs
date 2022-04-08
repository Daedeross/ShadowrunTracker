using ShadowrunTracker.Data;
using ShadowrunTracker.Model;
using ShadowrunTracker.ViewModels;
using System.Collections.Generic;

namespace ShadowrunTracker
{
    public interface IViewModelFactory
    {
        T Create<T>() where T : class, IViewModel;

        ICharacterViewModel Create(ICharacter character);

        IParticipantInitiativeViewModel Create(ICharacterViewModel character, InitiativeRoll initiative);

        IRequestInitiativesViewModel Create(IEnumerable<ICharacterViewModel> participants);

        ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel> participants);

        IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel> participants);
    }
}
