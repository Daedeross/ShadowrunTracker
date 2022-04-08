using ShadowrunTracker.ViewModels;
using System;

namespace ShadowrunTracker.Model
{
    public class NewCharacterEventArgs : EventArgs
    {
        public NewCharacterEventArgs(ICharacterViewModel character)
        {
            Character = character;
        }

        public ICharacterViewModel Character { get; }
    }
}
