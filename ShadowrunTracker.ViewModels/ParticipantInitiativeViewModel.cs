using ReactiveUI;
using ShadowrunTracker.Contract.Model;
using ShadowrunTracker.Contract.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ShadowrunTracker.ViewModels
{
    public class ParticipantInitiativeViewModel : ReactiveObject, IParticipantInitiativeViewModel, IDisposable
    {
        private readonly InitiativeRoll _roll; // values in it can mutate

        public ParticipantInitiativeViewModel(ICharacterViewModel character, InitiativeRoll initiativeRoll)
        {
            Character = character;
            _roll = initiativeRoll;
            m_InitiativeScore = _roll.Result;

            Character.PropertyChanged += OnCharacterPropertyChanged;
        }

        public ICharacterViewModel Character { get; private set; }

        private int m_InitiativeScore;

        public int InitiativeScore
        {
            get => m_InitiativeScore;
            set => this.RaiseAndSetIfChanged(ref m_InitiativeScore, value);
        }

        private bool m_SeizedInitiative;
        public bool SeizedInitiative
        {
            get => m_SeizedInitiative;
            set => this.RaiseAndSetIfChanged(ref m_SeizedInitiative, value);
        }

        private bool m_Acted;
        public bool Acted
        {
            get => m_Acted;
            set => this.RaiseAndSetIfChanged(ref m_Acted, value);
        }

        public InitiativeRoll GetNextPass()
        {
            if(!Acted)
            {
                throw new InvalidOperationException("Character has not yet acted");
            }

            return new InitiativeRoll
            {
                CurrentState = Character.CurrentState,
            }
        }

        #region Private Methods

        private void OnCharacterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, Character))
            {
                return;
            }

            switch (_roll.CurrentState)
            {
                case InitiativeState.Physical:
                    if (e.PropertyName == nameof(CharacterViewModel.AugmentedPhysicalInitiative))
                    {
                        if (_roll.ScoreUsed != Character.AugmentedPhysicalInitiative)
                        {
                            var diff = _roll.ScoreUsed - Character.AugmentedPhysicalInitiative;
                            _roll.ScoreUsed = Character.AugmentedPhysicalInitiative;
                            InitiativeScore += diff;
                        }
                    }
                    else if (e.PropertyName == nameof(CharacterViewModel.AugmentedPhysicalInitiativeDice))
                    {
                        if (_roll.DiceUsed != Character.AugmentedPhysicalInitiativeDice)
                        {
                            var diff = _roll.DiceUsed - Character.AugmentedPhysicalInitiativeDice;
                            _roll.DiceUsed = Character.AugmentedPhysicalInitiativeDice;

                            if (diff > 0)
                            {
                                InitiativeScore -= diff;
                            }

                            InitiativeScore += diff;
                        }
                    }
                    return;
                case InitiativeState.Astral:
                    if (e.PropertyName == nameof(CharacterViewModel.AugmentedAstralInitiative))
                    {
                        if (_roll.ScoreUsed != Character.AugmentedAstralInitiative)
                        {
                            var diff = _roll.ScoreUsed - Character.AugmentedAstralInitiative;
                            _roll.ScoreUsed = Character.AugmentedAstralInitiative;
                            InitiativeScore += diff;
                        }
                    }
                    else if (e.PropertyName == nameof(CharacterViewModel.AugmentedAstralInitiativeDice))
                    {
                        if (_roll.DiceUsed != Character.AugmentedAstralInitiativeDice)
                        {
                            var diff = _roll.DiceUsed - Character.AugmentedAstralInitiativeDice;
                            _roll.DiceUsed = Character.AugmentedAstralInitiativeDice;

                            if (diff > 0)
                            {
                                InitiativeScore -= diff;
                            }

                            InitiativeScore += diff;
                        }
                    }
                    break;
                case InitiativeState.MatrixAR:
                    break;
                case InitiativeState.MatrixCold:
                    break;
                case InitiativeState.MatrixHot:
                    break;
                default:
                    return;
            }
        }

        #endregion

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Character.PropertyChanged -= OnCharacterPropertyChanged;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
