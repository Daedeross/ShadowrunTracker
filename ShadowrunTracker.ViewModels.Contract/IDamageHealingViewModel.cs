namespace ShadowrunTracker.ViewModels
{
    public interface IDamageHealingViewModel : IDisposableModalViewModel<IDamageHealingViewModel>
    {
        ICharacterViewModel Character { get; }
        bool IsHealing { get; set; }
        int Physical { get; set; }
        int Stun { get; set; }
    }
}
