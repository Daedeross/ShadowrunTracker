using ShadowrunTracker.ViewModels;

namespace ShadowrunTracker.Mock
{
    public class MockDelayActionViewModel : DelayActionViewModel
    {
        public MockDelayActionViewModel()
            : base(TestData.TestCharacters.CreateParticipant("Bob", 8, 1))
        {
        }
    }
}
