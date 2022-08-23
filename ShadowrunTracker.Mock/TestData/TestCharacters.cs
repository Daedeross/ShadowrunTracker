using ShadowrunTracker.Data;
using ShadowrunTracker.Data.Traits;
using ShadowrunTracker.Model;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;

namespace ShadowrunTracker.Mock.TestData
{
    public static class TestCharacters
    {
        public static IDataStore<Guid> DataStore = TestData.TestCharacters.DataStore;

        public static Character Create(string alias, int initScore, int initDice, int edge = 1, int reaction = 1, int intuition = 1)
        {
            if (initDice < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(initDice));
            }

            var improvements = new List<Improvement>();
            if (initScore != reaction + intuition)
            {
                improvements.Add(new Improvement
                {
                    Name = "Initiative Bonus",
                    Target = nameof(ICharacterViewModel.PhysicalInitiative),
                    TargetKind = TraitKind.Attribute,
                    Value = initScore - reaction - intuition,
                });
            }

            if (initDice != 0)
            {
                improvements.Add(new Improvement
                {
                    Name = "Initiative Bonus",
                    Target = nameof(ICharacterViewModel.PhysicalInitiativeDice),
                    TargetKind = TraitKind.Attribute,
                    Value = initDice - 1
                });
            }

            return new Character
            {
                Alias = alias,
                BaseBody = 1,
                BaseAgility = 1,
                BaseReaction = reaction,
                BaseStrength = 1,
                BaseCharisma = 1,
                BaseIntuition = 1,
                BaseLogic = 1,
                BaseWillpower = 1,
                Edge = edge,
                Improvements = improvements,
                Skills = new List<Skill>()
            };
        }

        public static ICharacterViewModel CreateViewModel(string alias, int initScore, int initDice, int edge = 1, int reaction = 1, int intuition = 1)
            => new CharacterViewModel(Roller.Default, TestData.TestCharacters.DataStore, Create(alias, initScore, initDice, edge, reaction, intuition));

        public static IParticipantInitiativeViewModel CreateParticipant(ICharacterViewModel character, InitiativeRoll? roll = null)
        {
            if (roll is null)
            {
                roll = new InitiativeRoll
                {
                    CurrentState = InitiativeState.Physical,
                    DiceUsed = character.PhysicalInitiativeDice,
                    ScoreUsed = character.PhysicalInitiative,
                    Result = Roller.Default.RollDice(character.PhysicalInitiativeDice).Sum + character.PhysicalInitiative
                };
            }

            return new ParticipantInitiativeViewModel(TestData.TestCharacters.DataStore, character, roll);
        }

        public static IParticipantInitiativeViewModel CreateParticipant(string alias, int initScore, int initDice, InitiativeRoll? roll = null, int edge = 1, int reaction = 1, int intuition = 1)
        {
            if (initDice < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(initDice));
            }

            return CreateParticipant(CreateViewModel(alias, initScore, initDice, edge, reaction, intuition), roll);
        }

        public static IEnumerable<ICharacterViewModel> TestGroup =  new List<ICharacterViewModel>
        {
            TestCharacters.CreateViewModel("Alice", 12, 2),
            TestCharacters.CreateViewModel("Bob", 4, 1),
            TestCharacters.CreateViewModel("Sue", 18, 4),
            TestCharacters.CreateViewModel("John", 11, 3),
            TestCharacters.CreateViewModel("Tina", 13, 1),
        };
    }
}
