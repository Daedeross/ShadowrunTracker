namespace ShadowrunTracker.Contract.ViewModels
{
    public interface ILeveledTraitViewModel : ITraitViewModel
    {
        int Rating { get; set; }

        int BonusRating { get; set; }

        int AugmentedRating { get; set; }
    }
}
