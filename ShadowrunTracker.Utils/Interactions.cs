using ReactiveUI;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Utils
{
    public static class Interactions
    {
        public static Interaction<string, bool> ConfirmationRequest { get; } = new Interaction<string, bool>();

        public static Interaction<SaveContext, string> SaveDialog { get; } = new Interaction<SaveContext, string>();
    }
}
