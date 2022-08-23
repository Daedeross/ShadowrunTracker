using ShadowrunTracker.ViewModels;
using System;

namespace ShadowrunTracker.Mock
{
    public class MockEncounterViewModel : EncounterViewModel
    {
        public MockEncounterViewModel()
            : base(new MockViewModelFactory(), TestData.TestCharacters.DataStore)
        {
        }
    }
}
