namespace ShadowrunTracker
{
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public interface IDataStore<TKey> : INotifyCollectionChanged
        where TKey : IEquatable<TKey>
    {
        ConditionalValue<T> TryGet<T>(TKey key);

        ConditionalValue<object> TryGet(Type type, TKey key);

        IDictionary<TKey, T> GetAll<T>();

        void Set<T>(TKey key, T value);

        void Set(Type type, TKey key, object value);

        bool Remove(Type type, TKey key);

        void Clear();
    }
}
