namespace ShadowrunTracker.Model
{
    public struct ConditionalValue<TValue>
    {
        public bool HasValue { get; }

        public TValue Value { get; }

        public ConditionalValue(bool hasValue, TValue value)
        {
            HasValue = hasValue;
            Value = value;
        }
    }
}
