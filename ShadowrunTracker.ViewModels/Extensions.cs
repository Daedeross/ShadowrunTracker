using ShadowrunTracker.Data;
using ShadowrunTracker.Data.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static ISkill ToModel(this ISkillViewModel skill)
        {
            return new Skill
            {
                Name = skill.Name,
                LinkedAttribute = skill.LinkedAttribute,
                Description = skill.Description,
                Notes = skill.Notes,
                Page = skill.Page,
                Rating = skill.BaseRating,
                Source = skill.Source,
            };
        }

        public static IImprovement ToModel(this IImprovementViewModel improvement)
        {
            return new Improvement
            {
                Name = improvement.Name,
                Target = improvement.Target,
                TargetKind = improvement.TargetKind,
                Value = improvement.Value,
            };
        }

        public static ICharacter ToModel(this ICharacterViewModel character)
        {
            return new Character
            {
                Id = character.Id,
                Alias = character.Alias,
                IsPlayer = character.IsPlayer,
                Player = character.Player,
                Essence = character.Essence,
                BaseBody = character.BaseBody,
                BaseAgility = character.BaseAgility,
                BaseReaction = character.BaseReaction,
                BaseStrength = character.BaseStrength,
                BaseCharisma = character.BaseCharisma,
                BaseIntuition = character.BaseIntuition,
                BaseLogic = character.BaseLogic,
                BaseWillpower = character.BaseWillpower,
                Edge = character.Edge,
                Magic = character.Magic,
                Resonance = character.Resonance,
                PainEditor = character.PainEditor,
                PainResistence = character.PainResistence,
                SpellsSustained = character.SpellsSustained,
                Skills = character.Skills.Select(ToModel).ToList(),
                Improvements = character.Improvements.Select(ToModel).ToList()
            };
        }
    }
}
