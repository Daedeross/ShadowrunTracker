using ReactiveUI;
using ShadowrunTracker.Model;
using ShadowrunTracker.ViewModels.Traits;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public class NewCharacterViewModel : ReactiveObject
    {
        public NewCharacterViewModel()
        {
            Character = new CharacterViewModel(new Utils.Roller());
        }

        public NewCharacterViewModel(IRoller roller)
        {
            Character = new CharacterViewModel(roller);
        }

        public ICharacterViewModel Character { get; }

        public IReadOnlyCollection<InitiativeScoreViewModel> Initiatives { get; }
            = new List<InitiativeScoreViewModel>
            {
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.Physical,
                    Score = 2,
                    Dice = 1,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.Astral,
                    Score = 2,
                    Dice = 2,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.MatrixAR,
                    Score = 2,
                    Dice = 1,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.MatrixCold,
                    Score = 2,
                    Dice = 3,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.MatrixHot,
                    Score = 2,
                    Dice = 4,
                },
            };
    }
}
