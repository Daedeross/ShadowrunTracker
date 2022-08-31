namespace ShadowrunTracker.Model
{
    using System;

    public record class EntryDatum(string Name, Type? Type = null, string? Mask = null);
}
