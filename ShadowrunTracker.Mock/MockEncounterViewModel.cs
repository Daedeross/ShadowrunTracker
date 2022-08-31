using ShadowrunTracker.ViewModels;
using System;

namespace ShadowrunTracker.Mock
{
    public class MockEncounterViewModel : GmEncounterViewModel
    {
        public MockEncounterViewModel()
            : base(new MockViewModelFactory(), TestData.TestCharacters.DataStore)
        {
        }
    }
}
