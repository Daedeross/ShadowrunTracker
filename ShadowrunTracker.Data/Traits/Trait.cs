using ShadowrunTracker.Contract.Data;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data.Traits
{
    [DataContract]
    public class Trait : ITrait
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
