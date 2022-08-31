namespace ShadowrunTracker
{
    using System;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class TypeDiscriminatorAttribute : Attribute
    {
        public const string JsonProperty = "TypeDiscriminator";

        private int _discriminator;

        public TypeDiscriminatorAttribute(int discriminator)
        {
            _discriminator = discriminator;
        }

        public int Discriminator => _discriminator;
    }
}
