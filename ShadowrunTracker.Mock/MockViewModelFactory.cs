using ShadowrunTracker.Data;
using ShadowrunTracker.Model;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Mock
{
    public class MockViewModelFactory : IViewModelFactory
    {
        public ICharacterViewModel Create(ICharacter character)
        {
            return new CharacterViewModel(Roller.Default, character);
        }

        public IParticipantInitiativeViewModel Create(ICharacterViewModel character, InitiativeRoll initiative)
        {
            return new ParticipantInitiativeViewModel(character, initiative);
        }

        public IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel> participants)
        {
            return new InitiativePassViewModel(participants);
        }

        public IRequestInitiativesViewModel Create(IEnumerable<ICharacterViewModel> participants)
        {
            throw new NotImplementedException();
        }

        T IViewModelFactory.Create<T>()
        {
            throw new NotImplementedException();
        }

        public ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel> participants)
        {
            return new MockCombatRoundViewModel();
        }

        public void Release(IViewModel viewModel)
        {
            if (viewModel is IDisposable d)
            {
                d.Dispose();
            }
        }
    }
}
