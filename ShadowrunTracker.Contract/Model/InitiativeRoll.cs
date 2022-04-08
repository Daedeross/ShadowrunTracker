namespace ShadowrunTracker.Model
{
    public class InitiativeRoll
    {
        public int Result { get; set; }
        public int ScoreUsed { get; set; }
        public int DiceUsed { get; set; }
        public bool Blitzed { get; set; }
        public InitiativeState CurrentState { get; set; }
    }
}
