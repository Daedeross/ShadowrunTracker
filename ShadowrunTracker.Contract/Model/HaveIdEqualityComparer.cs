namespace ShadowrunTracker.Model
{
    using System.Collections.Generic;

    public class HaveIdEqualityComparer : IEqualityComparer<IHaveId>
    {
        public bool Equals(IHaveId x, IHaveId y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (x is null)
            {
                return false;
            }
            if (y is null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode(IHaveId obj)
        {
            return obj?.Id.GetHashCode() ?? 0;
        }
    }
}
