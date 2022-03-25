namespace ShadowrunTracker.Contract.Data
{
    public interface ILeveledTrait : ITrait
    {
        int Rating { get; set; }

        int BonusRating { get; set; }

        int AugmentedRating { get; set; }
    }
}
