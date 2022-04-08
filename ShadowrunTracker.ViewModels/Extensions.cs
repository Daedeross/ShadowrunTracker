using ShadowrunTracker.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public static class Extensions
    {
        public static void Improve(this ICharacter character, string propName, Func<ICharacter, int> propGetter, int target)
        {
            var value = propGetter(character);
            var diff = target - value;

            if (diff == 0)
            {
                return;
            }

            string prefix = Math.Sign(diff) > 0 ? "+" : string.Empty;

            var improvement = new Improvement
            {
                Name = $"{prefix}{diff} to {propName}",
                Target = propName,
                TargetKind = Model.TraitKind.Attribute,
                Value = diff
            };

            character.Improvements.Add(improvement);
        }

        public static void Improve(this ICharacterViewModel character, string propName, Func<ICharacterViewModel, int> propGetter, int target)
        {
            var value = propGetter(character);
            var diff = target - value;

            if (diff == 0)
            {
                return;
            }

            string prefix = Math.Sign(diff) > 0 ? "+" : string.Empty;

            var improvement = new Improvement
            {
                Name = $"{prefix}{diff} to {propName}",
                Target = propName,
                TargetKind = Model.TraitKind.Attribute,
                Value = diff
            };

            character.Improvements.Add(new ImprovementViewModel(improvement));
        }
    }
}
