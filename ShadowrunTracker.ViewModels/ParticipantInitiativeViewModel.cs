using ReactiveUI;
using ShadowrunTracker.Model;
using ShadowrunTracker.ViewModels;
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
            };
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
                    if (e.PropertyName == nameof(CharacterViewModel.PhysicalInitiative))
                    {
                        if (_roll.ScoreUsed != Character.PhysicalInitiative)
                        {
                            var diff = _roll.ScoreUsed - Character.PhysicalInitiative;
                            _roll.ScoreUsed = Character.PhysicalInitiative;
                            InitiativeScore += diff;
                        }
                    }
                    else if (e.PropertyName == nameof(CharacterViewModel.PhysicalInitiativeDice))
                    {
                        if (_roll.DiceUsed != Character.PhysicalInitiativeDice)
                        {
                            var diff = _roll.DiceUsed - Character.PhysicalInitiativeDice;
                            _roll.DiceUsed = Character.PhysicalInitiativeDice;

                            if (diff > 0)
                            {
                                InitiativeScore -= diff;
                            }

                            InitiativeScore += diff;
                        }
                    }
                    return;
                case InitiativeState.Astral:
                    if (e.PropertyName == nameof(CharacterViewModel.AstralInitiative))
                    {
                        if (_roll.ScoreUsed != Character.AstralInitiative)
                        {
                            var diff = _roll.ScoreUsed - Character.AstralInitiative;
                            _roll.ScoreUsed = Character.AstralInitiative;
                            InitiativeScore += diff;
                        }
                    }
                    else if (e.PropertyName == nameof(CharacterViewModel.AstralInitiativeDice))
                    {
                        if (_roll.DiceUsed != Character.AstralInitiativeDice)
                        {
                            var diff = _roll.DiceUsed - Character.AstralInitiativeDice;
                            _roll.DiceUsed = Character.AstralInitiativeDice;

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
