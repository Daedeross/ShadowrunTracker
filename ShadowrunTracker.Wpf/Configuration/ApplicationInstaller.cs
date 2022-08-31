namespace ShadowrunTracker.Wpf.Configuration
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.ModelBuilder.Inspectors;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using ShadowrunTracker.Client;
    using ShadowrunTracker.Communication;
    using ShadowrunTracker.Configuration;
    using ShadowrunTracker.Utils;
    using ShadowrunTracker.ViewModels;
    using ShadowrunTracker.Wpf.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;

    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            #region Configuration

            var builder = new ConfigurationManager()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            #endregion

            var propInjector = container.Kernel.ComponentModelBuilder
                         .Contributors
                         .OfType<PropertiesDependenciesModelInspector>()
                         .Single();
            container.Kernel.ComponentModelBuilder.RemoveContributor(propInjector);

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));

            container.AddFacility<TypedFactoryFacility>();
            container.AddFacility<DataStoreFacility>();

            container.Register(
                Classes.FromAssemblyContaining<OptionsBase>()
                    .BasedOn<OptionsBase>()
                    .WithServiceSelf()
                    .LifestyleSingleton(),
                Component.For<IConfigurationRoot>()
                    .Instance(configuration)
                    .LifestyleSingleton());

            AddOptions(container);

            container.Register(
                Classes.FromAssemblyNamed("ShadowrunTracker.ViewModels")
                    .BasedOn<IViewModel>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Component.For<IViewModelFactory>()
                    .AsFactory(),
                Component.For<IDrawerManager>()
                    .ImplementedBy<DrawerManager>()
                    .LifestyleSingleton());

            container.Register(
                Component.For<IRoller>()
                    .ImplementedBy<Roller>()
                    .LifestyleSingleton(),
                Component.For(typeof(IDataStore<>))
                    .ImplementedBy(typeof(TypedDataStore<>))
                    .LifestyleSingleton(),
                Component.For<HttpClient>()
                    .ImplementedBy<HttpClient>()
                    .LifestyleSingleton(),
                Component.For<IServiceFactory>()
                    .AsFactory(),
                Component.For<ICommunicationService>()
                    .ImplementedBy<CommunicationService>()
                    .LifestyleTransient(),
                Component.For<IRetryPolicy>()
                    .ImplementedBy<RetryPolicy>()
                    .DependsOn(Dependency.OnValue("maxRetries", -1),
                               Dependency.OnValue("maxWaitTime", 60))
                    .LifestyleSingleton());

            container.Register(
                Classes.FromAssemblyContaining<MainWindow>()
                    .BasedOn(typeof(ReactiveUI.IViewFor<>))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .Configure(ConfigureView)
                    .LifestyleTransient(),
                Component.For<IViewFactory>()
                    .AsFactory(new TypedFactoryNamedComponentSelector()),
                Component.For<ReactiveUI.IViewLocator>()
                    .ImplementedBy<WindsorViewLocator>()
                    .Named(nameof(WindsorViewLocator)));

            container.Register(
                Classes.FromAssemblyContaining<MainWindow>()
                    .BasedOn<ReactiveUI.IBindingTypeConverter>()
                    .WithServiceBase());

            container.Register(
                Classes.FromAssemblyContaining<ReactiveUI.CommandBinderImplementation>()
                    .IncludeNonPublicTypes()
                    .BasedOn<ReactiveUI.CommandBinderImplementation>()
                    .WithServiceDefaultInterfaces()
                    .Configure(r => r.Named("ICommandBinderImplementation"))
                    .LifestyleSingleton());
        }

        private void ConfigureView(ComponentRegistration c)
        {
            var nameAttr = c.Implementation.GetCustomAttribute<NameAttribute>();
            if (nameAttr is null)
            {
                return;
            }

            c.Named(nameAttr.Name);
        }

        private void AddOptions(IWindsorContainer container)
        {
            // should be equivalent to services.AddOptions()?
            // missing UnnamedOptionsManager<> because it's internal to MS's library
            container.Register(
                Component.For(typeof(IOptionsSnapshot<>))
                    .ImplementedBy(typeof(OptionsManager<>))
                    .LifestyleSingleton(),
                Component.For(typeof(IOptionsMonitor<>))
                    .ImplementedBy(typeof(OptionsMonitor<>))
                    .LifestyleSingleton(),
                Component.For(typeof(IOptionsFactory<>))
                    .ImplementedBy(typeof(OptionsFactory<>))
                    .LifestyleTransient(),
                Component.For(typeof(IOptionsMonitorCache<>))
                    .ImplementedBy(typeof(OptionsCache<>))
                    .LifestyleTransient());

            container.Register(
                Component.For(typeof(IOptionsChangeTokenSource<>))
                    .ImplementedBy(typeof(ConfigurationChangeTokenSource<>))
                    .DynamicParameters((kernel, context, args) =>
                    {
                        var name = context.GenericArguments[0].Name;
                        var root = kernel.Resolve<IConfigurationRoot>();
                        var config = root.GetSection(name);

                        args.AddTyped<IConfiguration>(config);

                        if (args["name"] == null)
                        {
                            args["name"] = Options.DefaultName;
                        }

                        return k => k.ReleaseComponent(root);
                    })
                    .LifestyleSingleton(),
                Component.For(typeof(IConfigureOptions<>))
                    .ImplementedBy(typeof(NamedConfigureFromConfigurationOptions<>))
                    .DynamicParameters((kernel, context, args) =>
                    {
                        var name = context.GenericArguments[0].Name;
                        var root = kernel.Resolve<IConfigurationRoot>();
                        var config = root.GetSection(name);

                        args.AddTyped<IConfiguration>(config);

                        if (args["name"] == null)
                        {
                            args["name"] = Options.DefaultName;
                        }

                        return k => k.ReleaseComponent(root);
                    })
                );
                    //.UsingFactoryMethod((kernel, model, context) =>
                    //{
                    //    model.Constructors
                    //    var root = kernel.Resolve<IConfigurationRoot>();
                    //    var config = root.GetSection(nameof(WebConfiguration));
                    //    return new ConfigurationChangeTokenSource(Options.DefaultName, config);
                    //});
        }
    }
}
