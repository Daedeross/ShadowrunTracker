namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data;

    public interface IEncounterViewModel : IRecordViewModel<Encounter>
    {
        ICombatRoundViewModel? CurrentRound { get; set; }
    }
}
