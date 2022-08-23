namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Model;

    public interface IInitiativeScoreViewModel : IViewModel
    {
        InitiativeState State { get; init; }
        int Score { get; set; }
        int Dice { get; set; }
    }
}
