using ShadowrunTracker.Model;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data.Traits
{
    [DataContract]
    public class Skill : LeveledTrait, ISkill
    {
        [DataMember]
        public SR5Attribute LinkedAttribute { get; set; }
    }
}
