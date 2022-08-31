namespace ShadowrunTracker.Data
{
    using ShadowrunTracker.Model;
    using System.Runtime.Serialization;

    [DataContract]
    [TypeDiscriminator(9)]
    public class Improvement : RecordBase
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public TraitKind TargetKind { get; set; }

        [DataMember]
        public string Target { get; set; } = string.Empty;

        [DataMember]
        public int Value { get; set; }
    }
}
