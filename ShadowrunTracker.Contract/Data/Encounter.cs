namespace ShadowrunTracker.Data
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    [TypeDiscriminator(1)]
    public class Encounter : RecordBase
    {
        [DataMember]
        public List<Character> Participants { get; set; } = new List<Character>();

        [DataMember]
        public List<CombatRound> Rounds { get; set; } = new List<CombatRound>();

        [DataMember]
        public int CurrentRoundIndex { get; set; }
    }
}
