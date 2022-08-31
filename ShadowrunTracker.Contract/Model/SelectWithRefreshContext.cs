namespace ShadowrunTracker.Model
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class SelectWithRefreshContext
    {
        public string Header { get; set; }
        public Func<CancellationToken, Task<IList<string>>> Refresh { get; set; }

        public SelectWithRefreshContext(string header, Func<CancellationToken, Task<IList<string>>> refresh)
        {
            Header = header;
            Refresh = refresh;
        }
    }
}
