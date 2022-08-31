namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System;

    public static class Extensions
    {
        public static IParticipantInitiativeViewModel Create(this IViewModelFactory factory, ICharacterViewModel character, InitiativeRoll initiative)
        {
            return factory.Create(character, new ParticipantInitiative { InitiativeRoll = initiative });
        }

        public static void Improve(this Character character, string propName, Func<Character, int> propGetter, int target)
        {
            var improvement = MakeImprovement(character, propName, propGetter, target);
            if (improvement is null)
            {
                return;
            }

            character.Improvements.Add(improvement);
        }

        public static void Improve(this ICharacterViewModel character, string propName, Func<ICharacterViewModel, int> propGetter, int target)
        {
            var improvement = character.MakeImprovement(propName, propGetter, target);
            if (improvement is null)
            {
                return;
            }

            character.Improvements.Add(new ImprovementViewModel(improvement));
        }

        public static Improvement? MakeImprovement<T>(this T character, string propName, Func<T, int> propGetter, int target)
        {
            var value = propGetter(character);
            var diff = target - value;

            if (diff == 0)
            {
                return null;
            }

            string prefix = Math.Sign(diff) > 0 ? "+" : string.Empty;

            return new Improvement
            {
                Name = $"{prefix}{diff} to {propName}",
                Target = propName,
                TargetKind = Model.TraitKind.Attribute,
                Value = diff
            };
        }
    }
}
