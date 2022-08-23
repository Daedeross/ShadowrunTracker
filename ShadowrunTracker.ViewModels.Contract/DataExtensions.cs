namespace ShadowrunTracker
{
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.ViewModels;
    using System.Runtime.CompilerServices;

    public static class DataExtensions
    {
        public static LeveledTrait ToModel(this ILeveledTraitViewModel viewModel)
        {
            return new LeveledTrait
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Notes = viewModel.Notes,
                Source = viewModel.Source,
                Page = viewModel.Page,
                BaseRating = viewModel.BaseRating,
                BonusRating = viewModel.BonusRating
            };
        }

        public static Skill ToModel(this ISkillViewModel skill)
        {
            return new Skill
            {
                Id = skill.Id,
                Name = skill.Name,
                LinkedAttribute = skill.LinkedAttribute,
                Description = skill.Description,
                Notes = skill.Notes,
                Page = skill.Page,
                BaseRating = skill.BaseRating,
                Source = skill.Source,
            };
        }

        public static Improvement ToModel(this IImprovementViewModel improvement)
        {
            return new Improvement
            {
                Id = improvement.Id,
                Name = improvement.Name,
                Target = improvement.Target,
                TargetKind = improvement.TargetKind,
                Value = improvement.Value,
            };
        }

        public static Character ToModel(this ICharacterViewModel character)
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

        public static TViewModel CreateOrUpdate<TRecord, TViewModel>(this IDataStore<Guid> store, TRecord record, Func<TRecord, TViewModel> factory)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : IHaveId
        {
            var cached = store.TryGet<TViewModel>(record.Id);
            if (cached.HasValue)
            {
                cached.Value.Update(record);
                return cached.Value;
            }
            else
            {
                var result = factory(record);
                store.Set(record.Id, result);
                return result;
            }
        }

        public static TViewModel UpdateOrThrow<TRecord, TViewModel>(this IDataStore<Guid> store, TRecord record)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : IHaveId
        {
            return store.CreateOrUpdate<TRecord, TViewModel>(record, r => throw new InvalidOperationException($"Cannot create a view model directly from {typeof(TRecord)} without context"));
        }

        public static IViewModel SyncFromRecord(this IDataStore<Guid> store, IViewModelFactory factory, RecordBase record) => record switch
        {
            Encounter encounter => store.CreateOrUpdate(encounter, r => factory.Create<IEncounterViewModel>()),
            Character character => store.UpdateOrThrow<Character, ICharacterViewModel>(character),
            CombatRound combatRound => store.UpdateOrThrow<CombatRound, ICombatRoundViewModel>(combatRound),
            ParticipantInitiative participant => store.UpdateOrThrow<ParticipantInitiative, IParticipantInitiativeViewModel>(participant),
            InitiativePass pass => store.UpdateOrThrow<InitiativePass, IInitiativePassViewModel>(pass),
            Skill skill => store.UpdateOrThrow<Skill, ISkillViewModel>(skill),
            Improvement improvement => store.UpdateOrThrow<Improvement, IImprovementViewModel>(improvement),

            _ => throw new InvalidOperationException()
        };
    }
}
