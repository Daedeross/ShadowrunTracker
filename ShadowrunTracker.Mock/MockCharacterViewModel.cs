using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Linq;

namespace ShadowrunTracker.Mock
{
    public class MockCharacterViewModel : CharacterViewModel
    {
        public MockCharacterViewModel()
            : base (new Roller(), TestData.TestCharacters.Create("Bob", 12, 2, 2, 6, 6))
        {
            IsPlayer = true;
            Player = "Sara";
        }
    }
}
