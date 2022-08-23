namespace ShadowrunTracker.Model
{
    using ShadowrunTracker.ViewModels;
    using System;

    public class NewCharacterEventArgs : EventArgs
    {
        public NewCharacterEventArgs(ICharacterViewModel character)
        {
            Character = character;
        }

        public ICharacterViewModel Character { get; }
    }
}
