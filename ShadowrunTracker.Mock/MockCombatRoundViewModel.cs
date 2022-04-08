using ShadowrunTracker.Mock.TestData;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowrunTracker.Mock
{
    public class MockCombatRoundViewModel : CombatRoundViewModel
    {
        public MockCombatRoundViewModel()
            : base(new MockViewModelFactory(), TestCharacters.TestGroup.Select(c => TestCharacters.CreateParticipant(c)))
        {
            foreach (var participant in TestCharacters.TestGroup)
            {
                AddParticipant(participant, participant.RollInitiative());
            }
        }
    }
}
