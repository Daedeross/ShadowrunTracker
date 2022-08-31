namespace ShadowrunTracker.Data
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    [TypeDiscriminator(3)]
    public class CombatRound : RecordBase
    {
        [DataMember]
        public List<ParticipantInitiative> Participants { get; set; } = new List<ParticipantInitiative>();

        [DataMember]
        public List<InitiativePass> InitiativePasses { get; set; } = new List<InitiativePass>();

        [DataMember]
        public int CurrentPassIndex { get; set; }
    }
}
