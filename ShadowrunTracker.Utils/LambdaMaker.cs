using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ShadowrunTracker.Utils
{
    public static class LambdaMaker
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _getterCache
            = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _setterCache
            = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

        public static Func<TProp> GetGetterWithClosure<TObj, TProp>(TObj obj, string propertyName)
        {
            var func = GetGetter<TObj, TProp>(propertyName);
            return () => func(obj);
        }

        public static Func<TObj, TProp> GetGetter<TObj, TProp>(string propertyName)
        {
            return (Func<TObj, TProp>)_getterCache.GetOrAdd(typeof(TObj), new ConcurrentDictionary<string, object>())
                .GetOrAdd(propertyName, GenerateGetterLambda<TObj, TProp>);
        }

        private static Func<TObj, TProp> GenerateGetterLambda<TObj, TProp>(string propertyName)
        {
            var propertyInfo = typeof(TObj).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(TObj)}'");
            }
            if (propertyInfo.PropertyType != typeof(TProp))
            {
                throw new ArgumentException($"The return type of '{propertyName}' does not match {typeof(TProp)}.");
            }
            if (!propertyInfo.CanRead)
            {
                throw new ArgumentException($"Property '{propertyName}' must be a readable property.");
            }

            var param = Expression.Parameter(typeof(TObj));

            return Expression.Lambda<Func<TObj, TProp>>(
                    Expression.Property(param, propertyInfo.GetMethod),
                    param)
                .Compile();
        }

        public static Action<TProp> GetSetterWithClosure<TObj, TProp>(TObj obj, string propertyName)
        {
            var func = GetSetter<TObj, TProp>(propertyName);
            return (v) => func(obj, v);
        }

        public static Action<TObj, TProp> GetSetter<TObj, TProp>(string propertyName)
        {
            return (Action<TObj, TProp>)_setterCache.GetOrAdd(typeof(TObj), new ConcurrentDictionary<string, object>())
                .GetOrAdd(propertyName, GenerateSetterLambda<TObj, TProp>);
        }

        private static Action<TObj, TProp> GenerateSetterLambda<TObj, TProp>(string propertyName)
        {
            var propertyInfo = typeof(TObj).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(TObj)}'");
            }
            if (propertyInfo.PropertyType != typeof(TProp))
            {
                throw new ArgumentException($"The return type of '{propertyName}' does not match {typeof(TProp)}.");
            }
            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException($"Property '{propertyName}' must be a writable property.");
            }

            var objParameter = Expression.Parameter(typeof(TObj));
            var valueParameter = Expression.Parameter(typeof(TProp));

            return Expression.Lambda<Action<TObj, TProp>>(
                    Expression.Call(objParameter, propertyInfo.SetMethod, valueParameter),
                    objParameter, valueParameter)
                .Compile();
        }
    }
}
