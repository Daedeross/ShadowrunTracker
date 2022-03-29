using ReactiveUI;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ShadowrunTracker.Utils
{
    public static class ViewModelExtensions
    {
        private static readonly Dictionary<Type, DependencyGraph> _typePropTrees
            = new Dictionary<Type, DependencyGraph>();

        public static DependencyGraph GetDependencyGraph(Type type)
        {
            if (!_typePropTrees.TryGetValue(type, out var graph))
            {
                graph = new DependencyGraph(type);
                _typePropTrees.Add(type, graph);
            }

            return graph;
        }

        public static ICollection<string> GetDependentPropertyNames(Type type, string propertyName)
        {
            return GetDependencyGraph(type)[propertyName];
        }

        /// <summary>
        /// RaiseAndSetIfChanged fully implements a Setter for a read-write
        /// property on a ReactiveObject, using CallerMemberName to raise the notification
        /// and the ref to the backing field to set the property.
        /// </summary>
        /// <typeparam name="TObj">The type of the This.</typeparam>
        /// <typeparam name="TRet">The type of the return value.</typeparam>
        /// <param name="reactiveObject">The <see cref="ReactiveObject"/> raising the notification.</param>
        /// <param name="backingField">A Reference to the backing field for this
        /// property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The name of the property, usually
        /// automatically provided through the CallerMemberName attribute.</param>
        /// <returns>The newly set value, normally discarded.</returns>
        public static TRet RaiseAndSetIfChanged<TObj, TRet>(
            this TObj reactiveObject,
            ref TRet backingField,
            TRet newValue,
            params string[] propertyNames)
            where TObj : IViewModel, IReactiveObject
        {
            if (propertyNames is null)
            {
                throw new ArgumentNullException(nameof(propertyNames));
            }

            if (propertyNames.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyNames));
            }

            if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                return newValue;
            }

            foreach (var name in propertyNames)
            {
                reactiveObject.RaisePropertyChanging(name);
            }
            backingField = newValue;
            foreach (var name in propertyNames)
            {
                reactiveObject.RaisePropertyChanged(name);
            }

            return newValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="backingField"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TRet SetAndRaiseIfChanged<TObj, TRet>(
            this TObj viewModel,
            ref TRet backingField,
            TRet newValue,
            [CallerMemberName] string? propertyName = null)
            where TObj : IViewModel, IReactiveObject
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                return newValue;
            }

            viewModel.RaiseChangingWithDependents(propertyName);
            backingField = newValue;
            viewModel.RaiseChangedWithDependents(propertyName);

            return newValue;
        }

        public static void RaiseChangingWithDependents<TObj>(this TObj viewModel, [CallerMemberName] string? propertyName = null)
            where TObj : IViewModel, IReactiveObject
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            foreach (var name in GetDependentPropertyNames(typeof(TObj), propertyName))
            {
                viewModel.RaisePropertyChanging(name);
            }
        }

        public static void RaiseChangedWithDependents<TObj>(this TObj viewModel, [CallerMemberName] string? propertyName = null)
            where TObj : IViewModel, IReactiveObject
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            foreach (var name in GetDependentPropertyNames(typeof(TObj), propertyName))
            {
                viewModel.RaisePropertyChanged(name);
            }
        }
    }
}
