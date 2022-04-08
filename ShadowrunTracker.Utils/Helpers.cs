using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Utils
{
    internal static class Helpers
    {
        public static ISet<string> IgnoredProperties { get; } = new HashSet<string>
        {
            nameof(ReactiveObject.Changed),
            nameof(ReactiveObject.Changing),
            nameof(ReactiveObject.ThrownExceptions),
        };
    }
}
