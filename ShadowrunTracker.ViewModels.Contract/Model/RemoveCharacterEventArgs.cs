namespace ShadowrunTracker.Model
{
    using ShadowrunTracker.ViewModels;
    using System;

    public class RemoveCharacterEventArgs : EventArgs
    {
        public ICharacterViewModel Character { get; }

        public RemoveCharacterEventArgs(ICharacterViewModel characterViewModel)
        {
            Character = characterViewModel ?? throw new ArgumentNullException(nameof(characterViewModel));
        }
    }
}
