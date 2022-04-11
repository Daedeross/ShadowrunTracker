using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Utils
{
    public static class Interactions
    {
        public static Interaction<string, bool> ConfirmationRequest { get; } = new Interaction<string, bool>();

        public static Interaction<string, bool> SaveDialog { get; } = new Interaction<string, bool>();

        public static Interaction<string, bool> SaveAsDialog { get; } = new Interaction<string, bool>();
    }
}
