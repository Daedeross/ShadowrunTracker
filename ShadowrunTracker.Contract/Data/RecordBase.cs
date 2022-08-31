namespace ShadowrunTracker.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(nameof(KnownTypes))]
    [TypeDiscriminator(0)]
    public abstract class RecordBase : IHaveId
    {
        private static readonly IEnumerable<Type> _knowTypes;
        static RecordBase()
        {
            _knowTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(RecordBase).IsAssignableFrom(t));
        }

        [DataMember(IsRequired = true, Order = 1)]
        public Guid Id { get; set; }

        public static IEnumerable<Type> KnownTypes() => _knowTypes;
    }
}
