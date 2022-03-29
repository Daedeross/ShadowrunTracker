using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;

namespace ShadowrunTools.Mock
{
    public class MockCharacterViewModel : CharacterViewModel
    {
        public MockCharacterViewModel()
            : base (new Roller())
        {

        }
    }
}
