namespace ShadowrunTracker.Communication
{
    using ShadowrunTracker.Data;
    using System.Runtime.Serialization;

    [DataContract]
    public class Update
    {
        [DataMember]
        public string? SessionName { get; set; }

        [DataMember]
        public RecordBase? Record { get; set; }
    }
}
