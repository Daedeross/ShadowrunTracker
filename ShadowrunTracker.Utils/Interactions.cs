namespace ShadowrunTracker.Utils
{
    using ReactiveUI;
    using ShadowrunTracker.Model;
    using System.Collections.Generic;
    using System.Reactive;

    public static class Interactions
    {
        public static Interaction<string, bool> ConfirmationRequest { get; } = new Interaction<string, bool>();

        public static Interaction<SaveContext, string> SaveDialog { get; } = new Interaction<SaveContext, string>();

        public static Interaction<(string Header, IList<EntryDatum> Options), IList<object?>?> GetData { get; } = new Interaction<(string, IList<EntryDatum>), IList<object?>?>();

        public static Interaction<SelectWithRefreshContext, string?> SelectFromList { get; } = new Interaction<SelectWithRefreshContext, string?>();
    }
}
