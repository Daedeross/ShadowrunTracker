namespace ShadowrunTracker.Data.Traits
{
    using ShadowrunTracker.Model;
    using System.Runtime.Serialization;

    [DataContract]
    [TypeDiscriminator(8)]
    public class Skill : LeveledTrait
    {
        [DataMember]
        public SR5Attribute LinkedAttribute { get; set; }
    }
}
