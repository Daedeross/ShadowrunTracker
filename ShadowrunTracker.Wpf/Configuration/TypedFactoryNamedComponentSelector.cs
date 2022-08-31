namespace ShadowrunTracker.Wpf.Configuration
{
    using Castle.Facilities.TypedFactory;
    using System.Reflection;

    public class TypedFactoryNamedComponentSelector : DefaultTypedFactoryComponentSelector
    {
        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            string? componentName = null;
            if (arguments != null && arguments.Length > 0)
            {
                componentName = arguments[0] as string;
            }

            if (string.IsNullOrEmpty(componentName))
                componentName = base.GetComponentName(method, arguments);

            return componentName;
        }
    }
}
