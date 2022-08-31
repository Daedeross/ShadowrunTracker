namespace ShadowrunTracker.Tests
{
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Mock;
    using ShadowrunTracker.Mock.TestData;
    using ShadowrunTracker.Utils;
    using ShadowrunTracker.ViewModels;
    using System.Collections.Generic;
    using Xunit;

    public class CharacterViewModelTests
    {
        private Character CreateLoader()
        {
            return new Character
            {
                Alias = "Test Character",
                IsPlayer = true,
                Player = "Bob",
                Essence = 6m,
                BaseBody = 3,
                BaseAgility = 2,
                BaseReaction = 4,
                BaseCharisma = 5,
                BaseIntuition = 6,
                BaseLogic = 1,
                BaseWillpower = 7,
                Edge = 3,
                Magic = 6,
                Resonance = 0,
                PainEditor = false,
                PainResistence = 1,
                SpellsSustained = 0,
                Skills = new List<Skill>
                {
                    new Skill
                    {
                        Name = "DoThing1",
                        LinkedAttribute = Model.SR5Attribute.Agility,
                        BaseRating = 2
                    },
                    new Skill
                    {
                        Name = "DoThing2",
                        LinkedAttribute = Model.SR5Attribute.Charisma,
                        BaseRating = 4
                    }
                }
            };
        }



        [Fact]
        public void CharacterCreatedFromLoaderTest()
        {
            var character = CreateLoader();
            var vm = new CharacterViewModel(new Roller(), TestCharacters.DataStore, new MockViewModelFactory(), character);

            Assert.Equal(character.Essence, vm.Essence);
            Assert.Equal(character.Player, vm.Player);
            Assert.Equal(character.IsPlayer, vm.IsPlayer);
            Assert.Equal(character.Alias, vm.Alias);

            Assert.Equal(character.BaseBody, vm.BaseBody);
            Assert.Equal(0, vm.BonusBody);
            Assert.Equal(character.BaseBody, vm.Body);
        }

        [Fact]
        public void CharacterLoadedWithImprovementsTest()
        {
            var character = CreateLoader();
            character.Improvements.Add(new Improvement
            {
                Name = "Bone Lacing",
                TargetKind = Model.TraitKind.Attribute,
                Target = "Body",
                Value = 1
            });

            var vm = new CharacterViewModel(new Roller(), TestCharacters.DataStore, new MockViewModelFactory(), character);

            Assert.Equal(character.Essence, vm.Essence);
            Assert.Equal(character.Player, vm.Player);
            Assert.Equal(character.IsPlayer, vm.IsPlayer);
            Assert.Equal(character.Alias, vm.Alias);

            Assert.Equal(3, vm.BaseBody);
            Assert.Equal(1, vm.BonusBody);
            Assert.Equal(4, vm.Body);
        }

        [Fact]
        public void CharacterAddedImprovementTest()
        {
            var character = CreateLoader();
            var improvement = new Improvement
            {
                Name = "Bone Lacing",
                TargetKind = Model.TraitKind.Attribute,
                Target = "Body",
                Value = 1
            };

            var vm = new CharacterViewModel(new Roller(), TestCharacters.DataStore, new MockViewModelFactory(), character);

            vm.Improvements.Add(new ImprovementViewModel(improvement));

            Assert.Equal(character.Essence, vm.Essence);
            Assert.Equal(character.Player, vm.Player);
            Assert.Equal(character.IsPlayer, vm.IsPlayer);
            Assert.Equal(character.Alias, vm.Alias);

            Assert.Equal(3, vm.BaseBody);
            Assert.Equal(1, vm.BonusBody);
            Assert.Equal(4, vm.Body);
        }
    }
}
