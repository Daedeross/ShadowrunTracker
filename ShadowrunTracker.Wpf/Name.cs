namespace ShadowrunTracker.Wpf
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class NameAttribute : Attribute
    {
        readonly string _name;

        public NameAttribute(string name)
        {
            _name = name;
        }

        public string Name => _name;

    }
}
