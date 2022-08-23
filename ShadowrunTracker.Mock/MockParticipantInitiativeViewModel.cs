using ShadowrunTracker.Mock.TestData;
using ShadowrunTracker.Model;
using ShadowrunTracker.ViewModels;
using System;

namespace ShadowrunTracker.Mock
{
    public class MockParticipantInitiativeViewModel : ParticipantInitiativeViewModel
    {
        public MockParticipantInitiativeViewModel()
            : base(TestData.TestCharacters.DataStore, new CharacterViewModel(Utils.Roller.Default, TestData.TestCharacters.DataStore, TestCharacters.Create("Test Character", 12, 2)), new InitiativeRoll { CurrentState = InitiativeState.Physical, DiceUsed = 2, ScoreUsed = 12, Result = 18 })
        {

        }
    }
}
