namespace ShadowrunTracker.Data
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class RecordBase : IHaveId
    {
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
    }
}
