using ShadowrunTracker.ViewModels;

namespace ShadowrunTracker.Mock
{
    public class MockEncounterViewModel : EncounterViewModel
    {
        public MockEncounterViewModel()
            : base(new MockViewModelFactory())
        {
        }
    }
}
