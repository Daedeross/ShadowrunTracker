namespace ShadowrunTracker.Data
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class InitiativePass : RecordBase
    {
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public List<Guid> ParticipantIds { get; set; } = new List<Guid>();

        [DataMember]
        public int ActiveParticipantIndex { get; set; }
    }
}
