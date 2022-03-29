﻿using ShadowrunTracker.Model;

namespace ShadowrunTracker.ViewModels
{
    public interface IParticipantInitiativeViewModel
    {
        ICharacterViewModel Character { get; }
        int InitiativeScore { get; set; }
        bool SeizedInitiative { get; set; }
        bool Acted { get; set; }
        InitiativeRoll GetNextPass();
    }
}