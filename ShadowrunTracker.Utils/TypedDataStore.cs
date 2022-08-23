namespace ShadowrunTracker
{
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TypedDataStore<TKey> : IDataStore<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<Type, Dictionary<TKey, object>> _dictionary
            = new();

        public void Clear()
        {
            foreach (var kvp in _dictionary)
            {
                kvp.Value.Clear();
                _dictionary.Remove(kvp.Key);
            }
        }

        public IDictionary<TKey, T> GetAll<T>()
        {
            if (!_dictionary.TryGetValue(typeof(T), out var dict))
            {
                dict = new Dictionary<TKey, object>();
                _dictionary.Add(typeof(T), dict);
            }

            return dict
                .ToDictionary(kvp => kvp.Key, kvp => (T)kvp.Value);
        }

        public bool Remove(Type type, TKey key)
        {
            if (_dictionary.TryGetValue(type, out var dict))
            {
                return dict.Remove(key);
            }

            return false;
        }

        public void Set<T>(TKey key, T value)
        {
            if (!_dictionary.TryGetValue(typeof(T), out var dict))
            {
                dict = new Dictionary<TKey, object>();
                _dictionary[typeof(T)] = dict;
            }
#pragma warning disable CS8601 // Possible null reference assignment.
            dict[key] = value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public void Set(Type type, TKey key, object value)
        {
            if (!type.IsAssignableFrom(value.GetType()))
            {
                throw new InvalidCastException($"{nameof(value)} is not of type {type}");
            }
            if (!_dictionary.TryGetValue(type, out var dict))
            {
                dict = new Dictionary<TKey, object>();
                _dictionary[type] = dict;
            }
#pragma warning disable CS8601 // Possible null reference assignment.
            dict[key] = value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public ConditionalValue<T> TryGet<T>(TKey key)
        {
            if (_dictionary.TryGetValue(typeof(T), out var dict))
            {
                if (dict.TryGetValue(key, out var result))
                {
                    if (result is T value)
                    {
                        return new ConditionalValue<T>(true, value);
                    }
                }
            }

            return default;
        }

        public ConditionalValue<object> TryGet(Type type, TKey key)
        {
            if (_dictionary.TryGetValue(type, out var dict))
            {
                if (dict.TryGetValue(key, out var result))
                {
                    if (type.IsAssignableFrom(result.GetType()))
                    {
                        return new ConditionalValue<object>(true, result);
                    }
                }
            }

            return default;
        }
    }
}
