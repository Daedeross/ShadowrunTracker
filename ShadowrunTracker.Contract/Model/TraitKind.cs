using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Contract.Model
{
    public enum TraitKind
    {
        None = 0,
        /// <summary>Includes special and derived attributes</summary>
        Attribute = 1,
        /// <summary>Active and Knowledge Skills</summary>
        Skill = 2,
    }
}
