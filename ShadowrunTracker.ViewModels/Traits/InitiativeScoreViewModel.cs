﻿namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker.Model;

    public class InitiativeScoreViewModel : ReactiveObject, IInitiativeScoreViewModel
    {
        public InitiativeState State { get; init; }

        private int m_Score;
        public int Score
        {
            get => m_Score;
            set => this.RaiseAndSetIfChanged(ref m_Score, value);
        }

        private int m_Dice;
        public int Dice
        {
            get => m_Dice;
            set => this.RaiseAndSetIfChanged(ref m_Dice, value);
        }
    }
}
