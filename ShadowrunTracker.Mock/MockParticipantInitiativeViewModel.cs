using ShadowrunTracker.Mock.TestData;
using ShadowrunTracker.Model;
using ShadowrunTracker.ViewModels;

namespace ShadowrunTracker.Mock
{
    public class MockParticipantInitiativeViewModel : ParticipantInitiativeViewModel
    {
        public MockParticipantInitiativeViewModel()
            : base(new CharacterViewModel(Utils.Roller.Default, TestCharacters.Create("Test Character", 12, 2)), new InitiativeRoll { CurrentState = InitiativeState.Physical, DiceUsed = 2, ScoreUsed = 12, Result = 18 })
        {

        }
    }
}
