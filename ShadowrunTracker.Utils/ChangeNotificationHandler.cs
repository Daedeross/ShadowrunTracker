namespace ShadowrunTracker.Utils
{
    using ReactiveUI;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ChangeNotificationHandler<T>: IDisposable
        where T : IViewModel, IReactiveObject
    {
        private static readonly Func<Dictionary<string, object?>> _cacheFactory;
        private static readonly IReadOnlyDictionary<string, Func<T, object>> _getters;
        private static readonly IReadOnlyDictionary<string, string[]> _propertyDependents;

        static ChangeNotificationHandler()
        {
            PropertyInfo[] properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(pi => pi.CanRead)
                .Where(pi => !Helpers.IgnoredProperties.Contains(pi.Name))
                .ToArray();

            _cacheFactory = GenerateDefaultCacheFactory(properties);
            _propertyDependents = properties
                .SelectMany(pi => pi.GetCustomAttributes<DependsOnAttribute>(true).Select(a => a.Name), (pi, dependsOn) => new { pi.Name, dependsOn })
                .GroupBy(a => a.dependsOn, a => a.Name)
                .ToDictionary(a => a.Key, a => a.ToArray());

            _getters = GenerateGetterLambdas(properties);
        }

        private readonly T _viewModel;

        private readonly Dictionary<string, object?> _valueCache;

        public ChangeNotificationHandler(T viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            _valueCache = _cacheFactory();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ReferenceEquals(_viewModel, sender))
            {
                NotifyDependentsChanged(e.PropertyName);
            }
        }

        private void NotifyDependentsChanged(string propertyName)
        {
            if (_propertyDependents.TryGetValue(propertyName, out var dependents))
            {
                foreach (var property in dependents)
                {
                    if (_valueCache.TryGetValue(property, out var oldValue))
                    {
                        if (_getters.TryGetValue(property, out var getter))
                        {
                            var newValue = getter(_viewModel);
                            if (!Equals(oldValue, newValue))
                            {
                                _valueCache[property] = newValue;
                                _viewModel.RaisePropertyChanged(property);
                            }
                        }
                    }
                }
            }
        }

        private static Func<Dictionary<string, object?>> GenerateDefaultCacheFactory(PropertyInfo[] properties)
        {
            var defaults = properties
                .ToDictionary(
                    a => a.Name,
                    a => (a.PropertyType.IsValueType ? Activator.CreateInstance(a.PropertyType) : null)
                );

            return () => defaults.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private static IReadOnlyDictionary<string, Func<T, object>> GenerateGetterLambdas(PropertyInfo[] properties)
        {
            return properties
                .ToDictionary(
                    p => p.Name,
                    p =>
                    {
                        var param = Expression.Parameter(typeof(T));

                        return Expression.Lambda<Func<T, object>>(
                                Expression.Convert(
                                    Expression.Property(param, p.GetMethod),
                                    typeof(object)),
                                param)
                            .Compile();
                    });
        }

        #region IDisposable

        private bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
        }

        #endregion
    }
}
