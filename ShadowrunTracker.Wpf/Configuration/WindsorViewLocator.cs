namespace ShadowrunTracker.Wpf.Configuration
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class WindsorViewLocator : IViewLocator
    {
        private readonly IViewFactory _viewFactory;
        private readonly MethodInfo _resolve;
        private readonly ConcurrentDictionary<(Type Type, string? Name), Func<IViewFor?>> _cache
            = new ConcurrentDictionary<(Type, string?), Func<IViewFor?>>();

        public WindsorViewLocator(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
            _resolve = typeof(WindsorViewLocator)
                .GetMethod(nameof(Resolve), BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                ?? throw new InvalidOperationException();
        }

        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            if (viewModel is null)
                return null;

            var type = viewModel.GetType();

            if (typeof(IViewModel).IsAssignableFrom(type))
            {
                var foo = GetView(type, contract);
                return foo;
            }

            return null;
        }

        private IViewFor<T> Resolve<T>(string? name)
            where T : class
        {
            return _viewFactory.For<T>(name);
        }

        private IViewFor? GetView(Type type, string? name)
        {
            return _cache.GetOrAdd((type, name), t => CreateDelegate(t.Type, t.Name))();
        }

        private Func<IViewFor?> CreateDelegate(Type type, string? name)
        {
            Type actualType;
            if (type.IsClass)
            {
                actualType = type.GetInterfaces()
                    .Where(x => string.Equals(x.Name, $"I{type.Name}"))
                    .Single();
            }
            else
            {
                actualType = type;
            }

            var mi = _resolve
                .MakeGenericMethod(actualType);

            //var param = Expression.Parameter(typeof(Type));

            return Expression.Lambda<Func<IViewFor?>>(
                Expression.Convert(
                    Expression.Call(Expression.Constant(this), mi, Expression.Constant(name, typeof(string))),
                    typeof(IViewFor)
                    )
                ).Compile();

        }
    }
}
