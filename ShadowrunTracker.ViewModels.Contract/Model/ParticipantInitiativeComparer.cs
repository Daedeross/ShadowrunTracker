namespace ShadowrunTracker.Model
{
    using ShadowrunTracker.ViewModels;
    using System.Collections.Generic;

    /// <summary>
    /// Compares two <see cref="IParticipantInitiativeViewModel"/> by who goes first.
    /// (i.e. x.CompareTo(y) returns 1 if x acts before y and likewize -1 if y acts before x)
    /// </summary>
    public class ParticipantInitiativeComparer : IComparer<IParticipantInitiativeViewModel>
    {
        public static IComparer<IParticipantInitiativeViewModel> Default { get; } = new ParticipantInitiativeComparer();

        public int Compare(IParticipantInitiativeViewModel x, IParticipantInitiativeViewModel y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            if (x is null)
            {
                return -1;
            }
            if (y is null)
            {
                return 1;
            }

            if (x.SeizedInitiative)
            {
                if (!y.SeizedInitiative)
                {
                    return 1;
                }
            }
            else if (y.SeizedInitiative)
            {
                return -1;
            }


            var score = x.InitiativeScore.CompareTo(y.InitiativeScore);
            if (score != 0)
            {
                return score;
            }

            var e = x.Character.Edge.CompareTo(y.Character.Edge);
            if (e != 0)
            {
                return e;
            }

            var r = x.Character.Reaction.CompareTo(y.Character.Reaction);
            if (r != 0)
            {
                return e;
            }

            var i = x.Character.Intuition.CompareTo(y.Character.Intuition);
            if (i != 0)
            {
                return i;
            }

            return x.TieBreaker.CompareTo(y.TieBreaker);
        }
    }
}
