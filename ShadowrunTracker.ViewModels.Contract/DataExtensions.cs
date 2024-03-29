﻿namespace ShadowrunTracker
{
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.ViewModels;
    using System.Runtime.InteropServices;

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
                Improvements = character.Improvements.Select(ToModel).ToList(),
                PhysicalDamage = character.PhysicalDamage,
                StunDamage = character.StunDamage,
            };
        }

        public static TViewModel CreateOrUpdate<TRecord, TViewModel>(this IDataStore<Guid> store, TRecord record, Func<TRecord, TViewModel> factory)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : IHaveId
        {
            try
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
            catch (Exception e)
            {

                throw;
            }
        }
        public static TViewModel CreateOrUpdate<TRecord, TViewModel>(this IDataStore<Guid> store, TRecord record, IViewModelFactory factory)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : RecordBase
        {
            return CreateOrUpdate(store, record, factory.Create<TViewModel, TRecord>);
        }

        public static TViewModel UpdateOrThrow<TRecord, TViewModel>(this IDataStore<Guid> store, TRecord record)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : IHaveId
        {
            return store.CreateOrUpdate<TRecord, TViewModel>(record, r => throw new InvalidOperationException($"Cannot create a view model directly from {typeof(TRecord)} without context"));
        }

        public static IViewModel SyncFromRecord(this IDataStore<Guid> store, IViewModelFactory factory, RecordBase record) => record switch
        {
            // only encounter can be created out of context
            Encounter encounter => store.CreateOrUpdate(encounter, factory.Create<IPlayerEncounterViewModel, Encounter>),
            // everyting else can only be created in context (i.e. in an update of their parent entity)
            // for now, it will throw an exception when an update comes in for an orphaned entity
            Character character => store.CreateOrUpdate(character, factory.Create<ICharacterViewModel, Character>),
            CombatRound combatRound => store.CreateOrUpdate(combatRound, r => factory.CreateRound(Array.Empty<IParticipantInitiativeViewModel>(), r)),
            ParticipantInitiative participant => store.CreateOrUpdate<ParticipantInitiative, IParticipantInitiativeViewModel>(participant, factory),
            InitiativePass pass => store.CreateOrUpdate(pass, r => factory.CreatePass(Array.Empty<IParticipantInitiativeViewModel>(), r)),
            Skill skill => store.CreateOrUpdate<Skill, ISkillViewModel>(skill, factory),
            Improvement improvement => store.CreateOrUpdate<Improvement, IImprovementViewModel>(improvement, factory),

            _ => throw new NotSupportedException()
        };
    }
}
