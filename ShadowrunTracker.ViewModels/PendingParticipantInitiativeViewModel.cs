using ReactiveUI;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public class PendingParticipantInitiativeViewModel : ViewModelBase, IPendingParticipantInitiativeViewModel
    {
        public ICharacterViewModel Character { get; }

        private bool m_Blitz;
        public bool Blitz
        {
            get => m_Blitz;
            set => this.RaiseAndSetIfChanged(ref m_Blitz, value);
        }

        private bool m_SiezeInitiative;
        public bool SiezeInitiative
        {
            get => m_SiezeInitiative;
            set => this.RaiseAndSetIfChanged(ref m_SiezeInitiative, value);
        }

        public int? Roll
        {
            get => m_InitiativeRoll?.Result;
            set
            {
                if (value is int v)
                {
                    if (m_InitiativeRoll is null)
                    {
                        var roll = new InitiativeRoll
                        {
                            CurrentState = Character.CurrentState,
                            ScoreUsed = Character.CurrentInitiative,
                            DiceUsed = Character.CurrentInitiativeDice,
                            Result = v
                        };
                        InitiativeRoll = roll;
                    }
                    else
                    {
                        m_InitiativeRoll.Result = v;
                    }
                }
                else
                {
                    m_InitiativeRoll = null;
                }
                this.RaisePropertyChanged();
            }
        }

        private InitiativeRoll? m_InitiativeRoll;
        public InitiativeRoll? InitiativeRoll
        {
            get => m_InitiativeRoll;
            protected set
            {
                this.RaiseAndSetIfChanged(ref m_InitiativeRoll, value);
                this.RaisePropertyChanged(nameof(Roll));
            }
        }

        public ICommand RollInitiativeCommand { get; }

        public PendingParticipantInitiativeViewModel(ICharacterViewModel character)
        {
            Character = character;
            RollInitiativeCommand = ReactiveCommand.Create(RollInitiative)
                .DisposeWith(_disposables);
        }

        private void RollInitiative()
        {
            InitiativeRoll = Character.RollInitiative(m_Blitz);
        }
    }
}
