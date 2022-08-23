namespace ShadowrunTracker.Model
{
    using System.Runtime.Serialization;

    [DataContract]
    public class InitiativeRoll
    {
        [DataMember]
        public int Result { get; set; }
        [DataMember]
        public int ScoreUsed { get; set; }
        [DataMember]
        public int DiceUsed { get; set; }
        [DataMember]
        public bool Blitzed { get; set; }
        [DataMember]
        public InitiativeState CurrentState { get; set; }
    }
}
