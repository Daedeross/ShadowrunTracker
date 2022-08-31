using ShadowrunTracker.Mock.TestData;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShadowrunTracker.Mock
{
    public class MockInitiativePassViewModel : InitiativePassViewModel
    {
        public MockInitiativePassViewModel()
            : base(TestData.TestCharacters.DataStore, TestData.TestCharacters.TestGroup.Select(c => TestCharacters.CreateParticipant(c)))
        {
            Participants[1].Character.BaseBody = 5;
            Participants[1].Character.ApplyPhysicalDamage(2);
            Participants[1].Character.IsPlayer = true;
            Participants[1].Character.Player = "Beyonce";
            Participants[2].Character.IsPlayer = true;
            Participants[2].Character.Player = "Jay-Z";
            Next();
        }
    }
}
