using ReactiveUI;
using ShadowrunTracker.Model;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ShadowrunTracker.ViewModels
{
    public class ParticipantInitiativeViewModel : ReactiveObject, IParticipantInitiativeViewModel, IDisposable
    {
        private const int ActionCost = 10;

        private static ISet<string> WatchedProperties = new HashSet<string>
        {
            nameof(ICharacterViewModel.CurrentState),
            nameof(ICharacterViewModel.CurrentInitiative),
            nameof(ICharacterViewModel.CurrentInitiativeDice),
            nameof(ICharacterViewModel.PhysicalBoxes),
            nameof(ICharacterViewModel.PhysicalDamage),
            nameof(ICharacterViewModel.StunBoxes),
            nameof(ICharacterViewModel.StunDamage),
        };

        private readonly InitiativeRoll _roll; // values in it can mutate

        private IRoller? _roller;
        /// <summary>
        /// Public to allow to be injected;
        /// </summary>
        public IRoller Roller { get => _roller ?? ShadowrunTracker.Utils.Roller.Default; set => _roller = value; }

        public ParticipantInitiativeViewModel(ICharacterViewModel character, InitiativeRoll initiativeRoll)
        {
            Character = character;
            _roll = initiativeRoll;
            m_InitiativeScore = _roll.Result;

            Character.PropertyChanged += OnCharacterPropertyChanged;
            SetPhysicalTrack();
            SetStunTrack();
        }

        public ICharacterViewModel Character { get; private set; }

        private int m_InitiativeScore;

        public int InitiativeScore
        {
            get => m_InitiativeScore;
            set => this.SetAndRaiseIfChanged(ref m_InitiativeScore, value);
        }

        private bool m_SeizedInitiative;
        public bool SeizedInitiative
        {
            get => m_SeizedInitiative;
            set => this.SetAndRaiseIfChanged(ref m_SeizedInitiative, value);
        }

        private bool m_Acted;
        public bool Acted
        {
            get => m_Acted;
            set => this.SetAndRaiseIfChanged(ref m_Acted, value);
        }

        private int? m_TieBreaker;
        public int TieBreaker
        {
            get
            {
                if (m_TieBreaker is null)
                {
                    m_TieBreaker = Roller.Next();
                }

                return m_TieBreaker.Value;
            }
        }

        [DependsOn(nameof(Acted))]
        [DependsOn(nameof(InitiativeScore))]
        public bool CanAct => !Acted
                           && InitiativeScore > 0
                           && Character.PhysicalBoxes > Character.PhysicalDamage
                           && Character.StunBoxes > Character.StunDamage;

        public ObservableCollection<IDamageBoxViewModel> PhysicalBoxes { get; } = new ObservableCollection<IDamageBoxViewModel>();
        public ObservableCollection<IDamageBoxViewModel> StunBoxes { get; } = new ObservableCollection<IDamageBoxViewModel>();

        public InitiativeRoll GetNextPass()
        {
            if (!Acted)
            {
                throw new InvalidOperationException("Character has not yet acted");
            }

            return new InitiativeRoll
            {
                CurrentState = Character.CurrentState,
                Result = InitiativeScore - ActionCost,
                DiceUsed = _roll.DiceUsed,
                ScoreUsed = _roll.ScoreUsed,
            };
        }

        #region Private Methods

        private void OnCharacterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(ReferenceEquals(sender, Character) && WatchedProperties.Contains(e.PropertyName)))
            {
                return;
            }

            if (e.PropertyName == nameof(ICharacterViewModel.PhysicalBoxes) || e.PropertyName == nameof(ICharacterViewModel.PhysicalDamage))
            {
                SetPhysicalTrack();
            }
            else if (e.PropertyName == nameof(ICharacterViewModel.StunBoxes) || e.PropertyName == nameof(ICharacterViewModel.StunDamage))
            {
                SetStunTrack();
            }
            else
            {
                _roll.CurrentState = Character.CurrentState;
                int newScore = Character.CurrentInitiative;
                int newDice = Character.CurrentInitiativeDice;

                if (newScore != _roll.ScoreUsed)
                {
                    InitiativeScore += newScore - _roll.ScoreUsed;
                    _roll.ScoreUsed = newScore;
                }

                if (!_roll.Blitzed)
                {
                    var diceDiff = newDice - _roll.DiceUsed;
                    if (diceDiff != 0)
                    {
                        InitiativeScore += Roller.RollDice(Math.Abs(diceDiff)).Sum * Math.Sign(diceDiff);
                        _roll.DiceUsed = newDice;
                    }

                }
            }
        }

        private void SetPhysicalTrack()
        {
            foreach (var item in PhysicalBoxes)
            {
                item.PropertyChanged -= OnPhysicalTrackChanged;
            }
            PhysicalBoxes.Clear();
            for (int i = 0; i < Character.PhysicalBoxes; i++)
            {
                var vm = new DamageBoxViewModel
                {
                    IsFilled = i < Character.PhysicalDamage,
                };
                vm.PropertyChanged += OnPhysicalTrackChanged;
                PhysicalBoxes.Add(vm);
            }
            this.RaisePropertyChanged(nameof(CanAct));
        }

        private void SetStunTrack()
        {
            foreach (var item in StunBoxes)
            {
                item.PropertyChanged -= OnStunTrackChanged;
            }
            StunBoxes.Clear();
            for (int i = 0; i < Character.StunBoxes; i++)
            {
                var vm = new DamageBoxViewModel
                {
                    IsFilled = i < Character.StunDamage,
                };
                vm.PropertyChanged += OnStunTrackChanged;
                StunBoxes.Add(vm);
            }
            this.RaisePropertyChanged(nameof(CanAct));
        }

        private void OnPhysicalTrackChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IDamageBoxViewModel.IsHovered))
            {
                SetHighlight(PhysicalBoxes);
            }
        }

        private void OnStunTrackChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IDamageBoxViewModel.IsHovered))
            {
                SetHighlight(StunBoxes);
            }
        }

        private void SetHighlight(IList<IDamageBoxViewModel> track)
        {
            int index = -1;
            for (int i = 0; i < track.Count; i++)
            {
                if (track[i].IsHovered)
                {
                    index = i;
                    break;
                }
            }
            for (int i = 0; i < track.Count; i++)
            {
                track[i].ShouldHighlight = i <= index;
            }
        }

        private void UpdateHover(object sender, PropertyChangedEventArgs e)
        {

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
