namespace ShadowrunTracker.Model
{
    public enum EventKind
    {
        None = 0,
        Roll = 1,
        InitiativeRoll = 2,
        InterruptAction = 3,
        Damage = 4,
        Healing = 5,
        ApplyImprovement = 6,
        ChangeState = 7,
        KO = 8
    }
}
