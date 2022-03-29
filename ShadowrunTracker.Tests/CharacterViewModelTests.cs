using ShadowrunTracker.Data;
using ShadowrunTracker.Data.Traits;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShadowrunTracker.Tests
{
    public class CharacterViewModelTests
    {
        private ICharacter CreateLoader()
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
                Skills = new List<ISkill>
                {
                    new Skill
                    {
                        Name = "DoThing1",
                        LinkedAttribute = Model.SR5Attribute.Agility,
                        Rating = 2
                    },
                    new Skill
                    {
                        Name = "DoThing2",
                        LinkedAttribute = Model.SR5Attribute.Charisma,
                        Rating = 4
                    }
                }
            };
        }



        [Fact]
        public void CharacterCreatedFromLoaderTest()
        {
            var character = CreateLoader();
            var vm = new CharacterViewModel(new Roller(), character);

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

            var vm = new CharacterViewModel(new Roller(), character);

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

            var vm = new CharacterViewModel(new Roller(), character);

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
