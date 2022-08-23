namespace ShadowrunTracker.Data
{
    using ShadowrunTracker.Model;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ParticipantInitiative : RecordBase
    {
        [DataMember]
        public Guid CharacterId { get; set; }

        [DataMember]
        public int InitiativeScore { get; set; }

        [DataMember]
        public bool SeizedInitiative { get; set; }

        [DataMember]
        public bool Acted { get; set; }

        [DataMember]
        public int TieBreaker { get; set; }

        [DataMember]
        public InitiativeRoll? InitiativeRoll { get; set; }
    }
}
