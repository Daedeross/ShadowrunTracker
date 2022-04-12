using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShadowrunTracker.Utils
{
    public class DependencyGraph
    {
        private class TreeNode<T>
        {
            public T? Value { get; set; }
            public List<TreeNode<T>> Children { get; } = new List<TreeNode<T>>();
        }

        private readonly Type _type;

        private readonly IDictionary<string, ICollection<string>> _propsWithChildren;

        public DependencyGraph(Type type)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _propsWithChildren = GenerateGraph();
        }

        public ICollection<string> this[string key]
        {
            get => _propsWithChildren[key];
        }

        private IDictionary<string, ICollection<string>> GenerateGraph()
        {
            var props = _type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            var propsWithParents = props.Select(pi => (pi.Name, Parents: GetParentProperties(pi)));

            var propNodes = propsWithParents.ToDictionary(
                tuple => tuple.Name,
                tuple => new TreeNode<string> { Value = tuple.Name });

            foreach (var (name, parents) in propsWithParents)
            {
                var cur = propNodes[name];
                foreach (var parent in parents)
                {
                    propNodes[parent].Children.Add(cur);
                }
            }

            var flatDependents = propNodes.ToDictionary(
                kvp => kvp.Key,
                kvp => FlattenChildren(kvp.Value));

            return flatDependents;
        }

        private static string[] GetParentProperties(PropertyInfo prop)
        {
            return prop.GetCustomAttributes<DependsOnAttribute>()
                .Select(a => a.Name).ToArray();
        }

        private static ICollection<T> FlattenChildren<T>(TreeNode<T> node)
        {
            var children = new HashSet<T> { };

            foreach (var child in node.Children)
            {
                children.UnionWith(FlattenChildren(child));
            }

            if (children.Contains(node.Value))
            {
                throw new InvalidOperationException("Cycle detected");
            }

            children.Add(node.Value);

            return children;
        }
    }
}
