using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowrunTracker.Wpf.Configuration
{
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

            container.Register(
                Classes.FromAssemblyNamed("ShadowrunTracker.ViewModels")
                    .BasedOn<IViewModel>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Component.For<IViewModelFactory>()
                    .AsFactory());

            container.Register(
                Component.For<IRoller>()
                    .ImplementedBy<Roller>()
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
