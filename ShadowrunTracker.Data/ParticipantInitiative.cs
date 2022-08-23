namespace ShadowrunTracker.Data
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ParticipantInitiative
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
    }
}
