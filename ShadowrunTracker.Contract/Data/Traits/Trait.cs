namespace ShadowrunTracker.Data.Traits
{
    using System.Runtime.Serialization;

    [DataContract]
    [TypeDiscriminator(6)]
    public abstract class Trait : RecordBase
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Description { get; set; } = string.Empty;

        [DataMember]
        public string Notes { get; set; } = string.Empty;

        [DataMember]
        public string Source { get; set; } = string.Empty;

        [DataMember]
        public int Page { get; set; }
    }
}
