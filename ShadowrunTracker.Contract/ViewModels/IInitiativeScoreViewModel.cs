using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface IInitiativeScoreViewModel
    {
        InitiativeState State { get; init; }
        int Score { get; set; }
        int Dice { get; set; }
    }
}
