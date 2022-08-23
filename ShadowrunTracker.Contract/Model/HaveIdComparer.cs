namespace ShadowrunTracker.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HaveIdComparer : IComparer<IHaveId>
    {
        public int Compare(IHaveId x, IHaveId y)
        {
            var xid = x?.Id ?? Guid.Empty;
            var yid = y?.Id ?? Guid.Empty;

            return xid.CompareTo(yid);
        }
    }
}
