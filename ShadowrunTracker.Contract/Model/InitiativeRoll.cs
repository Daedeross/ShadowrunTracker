namespace ShadowrunTracker.Contract.Model
{
    public class InitiativeRoll
    {
        public int Result { get; set; }
        public int ScoreUsed { get; set; }
        public int DiceUsed { get; set; }
        public InitiativeState CurrentState { get; set; }
    }
}
