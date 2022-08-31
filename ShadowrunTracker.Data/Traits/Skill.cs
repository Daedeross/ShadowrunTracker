namespace ShadowrunTracker.Data.Traits
{
    using ShadowrunTracker.Model;
    using System.Runtime.Serialization;

    [DataContract]
    public class Skill : LeveledTrait
    {
        [DataMember]
        public SR5Attribute LinkedAttribute { get; set; }
    }
}
