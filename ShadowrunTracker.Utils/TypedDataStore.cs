namespace ShadowrunTracker
{
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public class TypedDataStore<TKey> : IDataStore<TKey>, INotifyCollectionChanged
        where TKey : IEquatable<TKey>
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<TKey, object>> _dictionary
            = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void Clear()
        {
            foreach (var kvp in _dictionary)
            {
                kvp.Value.Clear();
                _dictionary.TryRemove(kvp.Key, out var _);
            }
            RaiseCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public IDictionary<TKey, T> GetAll<T>()
        {
            var dict = _dictionary.GetOrAdd(typeof(T), key => new ConcurrentDictionary<TKey, object>());

            return dict
                .ToDictionary(kvp => kvp.Key, kvp => (T)kvp.Value);
        }

        public bool Remove(Type type, TKey key)
        {
            if (_dictionary.TryGetValue(type, out var dict))
            {
                if(dict.TryRemove(key, out var oldValue))
                {
                    RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, default, oldValue);
                    return true;
                }
            }

            return false;
        }

        public void Set<T>(TKey key, T value)
        {
            var dict = _dictionary.GetOrAdd(typeof(T), key => new ConcurrentDictionary<TKey, object>());
#pragma warning disable CS8604 // Possible null reference argument.
            AddOrUpdate(dict, key, value);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public void Set(Type type, TKey key, object value)
        {
            var dict = _dictionary.GetOrAdd(type, key => new ConcurrentDictionary<TKey, object>());
            AddOrUpdate(dict, key, value);
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

        private void AddOrUpdate(ConcurrentDictionary<TKey, object> dict, TKey key, object value)
        {
            object? oldValue = default;
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Move;
            dict.AddOrUpdate(key, k =>
            {
                action = NotifyCollectionChangedAction.Add;
                return value;
            },
            (k, old) =>
            {
                oldValue = k;
                action = NotifyCollectionChangedAction.Replace;
                return value;
            });

            RaiseCollectionChanged(action, value, oldValue);
        }

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, object? newItem = default, object? oldItem = default)
        {
            NotifyCollectionChangedEventArgs args;
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    args = new NotifyCollectionChangedEventArgs(action, newItem);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    args = new NotifyCollectionChangedEventArgs(action, oldItem);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    args = new NotifyCollectionChangedEventArgs(action, newItem, oldItem);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    args = new NotifyCollectionChangedEventArgs(action);
                    break;
                default:
                    throw new NotSupportedException($"{action} not supported");
            }

            CollectionChanged?.Invoke(this, args);
        }
    }
}
