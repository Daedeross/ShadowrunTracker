namespace ShadowrunTracker.Model
{
    using ShadowrunTracker.ViewModels;
    using System.Collections.Generic;

    /// <summary>
    /// Identicial to <see cref="ParticipantInitiativeComparer"/> except for sorting in reverse order (first to last)
    /// </summary>
    public class ParticipantInitiativeReverseComparer : IComparer<IParticipantInitiativeViewModel>
    {
        public static IComparer<IParticipantInitiativeViewModel> Default { get; } = new ParticipantInitiativeReverseComparer();

        public int Compare(IParticipantInitiativeViewModel x, IParticipantInitiativeViewModel y)
        {
            if (ReferenceEquals(y, x))
            {
                return 0;
            }
            if (y is null)
            {
                return -1;
            }
            if (x is null)
            {
                return 1;
            }

            if (x.SeizedInitiative)
            {
                if (!y.SeizedInitiative)
                {
                    return -1;
                }
            }
            else if (y.SeizedInitiative)
            {
                return 1;
            }

            var score = y.InitiativeScore.CompareTo(x.InitiativeScore);
            if (score != 0)
            {
                return score;
            }

            var e = y.Character.Edge.CompareTo(x.Character.Edge);
            if (e != 0)
            {
                return e;
            }

            var r = y.Character.Reaction.CompareTo(x.Character.Reaction);
            if (r != 0)
            {
                return e;
            }

            var i = y.Character.Intuition.CompareTo(x.Character.Intuition);
            if (i != 0)
            {
                return i;
            }

            return y.TieBreaker.CompareTo(x.TieBreaker);
        }
    }
}
