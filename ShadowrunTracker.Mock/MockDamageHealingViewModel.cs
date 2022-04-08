using ShadowrunTracker.ViewModels;

namespace ShadowrunTracker.Mock
{
    public class MockDamageHealingViewModel : DamageHealingViewModel
    {
        public MockDamageHealingViewModel()
            : base(TestData.TestCharacters.CreateViewModel("Bob", 8, 1))
        {

        }
    }
}
