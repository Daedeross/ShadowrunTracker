namespace ShadowrunTracker.ViewModels
{
    public interface ILeveledTraitViewModel : ITraitViewModel
    {
        int BaseRating { get; set; }

        int BonusRating { get; set; }

        int AugmentedRating { get; }
    }
}
