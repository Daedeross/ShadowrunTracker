using DynamicData.Binding;
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
        public ICharacterViewModel Create(Character character)
        {
            return new CharacterViewModel(Roller.Default, TestData.TestCharacters.DataStore, character);
        }

        public IParticipantInitiativeViewModel Create(ICharacterViewModel character, InitiativeRoll initiative)
        {
            return new ParticipantInitiativeViewModel(TestData.TestCharacters.DataStore, character, initiative);
        }

        public IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel> participants)
        {
            return new InitiativePassViewModel(TestData.TestCharacters.DataStore, participants);
        }

        public IRequestInitiativesViewModel Create(IObservableCollection<ICharacterViewModel> characters)
        {
            throw new NotImplementedException();
        }

        T IViewModelFactory.Create<T>()
        {
            throw new NotImplementedException();
        }

        public ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel> characters)
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
