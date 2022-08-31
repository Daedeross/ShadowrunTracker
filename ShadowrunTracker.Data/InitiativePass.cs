namespace ShadowrunTracker.Data
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class InitiativePass
    {
        [DataMember]
        public List<ParticipantInitiative> Participants { get; set; } = new List<ParticipantInitiative>();

        [DataMember]
        public int ActiveParticipantIndex { get; set; }
    }
}
