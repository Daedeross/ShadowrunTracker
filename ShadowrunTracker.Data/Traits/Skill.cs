using ShadowrunTracker.Model;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data.Traits
{
    [DataContract]
    public class Skill : LeveledTrait, ISkill
    {
        [DataMember]
        public SR5Attribute LinkedAttribute { get; set; }

        public Skill()
        {

        }

        public Skill(ISkill s)
        {
            Name = s.Name;
            Description = s.Description;
            Notes = s.Notes;
            Source = s.Source;
            Page = s.Page;

            Rating = s.Rating;
        }
    }
}
