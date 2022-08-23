namespace ShadowrunTracker.Wpf.Configuration
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.ModelBuilder.Inspectors;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using ShadowrunTracker.Utils;
    using ShadowrunTracker.ViewModels;
    using ShadowrunTracker.Wpf.Helpers;
    using System.Linq;

    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var propInjector = container.Kernel.ComponentModelBuilder
                         .Contributors
                         .OfType<PropertiesDependenciesModelInspector>()
                         .Single();
            container.Kernel.ComponentModelBuilder.RemoveContributor(propInjector);

            container.AddFacility<TypedFactoryFacility>();
            container.AddFacility<DataStoreFacility>();

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
                    .LifestyleSingleton());

            container.Register(
                Classes.FromAssemblyContaining<MainWindow>()
                    .BasedOn(typeof(ReactiveUI.IViewFor<>))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                Component.For<IViewFactory>()
                    .AsFactory(),
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
    }
}
