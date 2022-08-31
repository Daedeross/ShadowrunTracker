namespace ShadowrunTracker.Wpf.Configuration
{
    using Castle.MicroKernel.Facilities;
    using ShadowrunTracker.ViewModels;
    using System;

    public class DataStoreFacility : AbstractFacility
    {
        private readonly Lazy<IDataStore<Guid>> _store;

        public DataStoreFacility()
            : base()
        {
            _store = new Lazy<IDataStore<Guid>>(() => Kernel.Resolve<IDataStore<Guid>>(), true);
        }

        protected override void Init()
        {
            Kernel.ComponentCreated += ComponentCreated;
            Kernel.ComponentDestroyed += ComponentDestroyed;
        }

        void ComponentCreated(Castle.Core.ComponentModel model, object instance)
        {
            if (instance is IHaveId haveId)
            {
                foreach (var service in model.Services)
                {
                    if (typeof(IHaveId).IsAssignableFrom(service) && typeof(IViewModel).IsAssignableFrom(service))
                    {
                        _store.Value.Set(service, haveId.Id, haveId);
                    }
                }
            }
        }

        void ComponentDestroyed(Castle.Core.ComponentModel model, object instance)
        {
            if (instance is IHaveId haveId)
            {
                foreach (var service in model.Services)
                {
                    if (typeof(IHaveId).IsAssignableFrom(service) && typeof(IViewModel).IsAssignableFrom(service))
                    {
                        _store.Value?.Remove(service, haveId.Id);
                    }
                }
            }
        }

        protected override void Dispose()
        {
            Kernel?.ReleaseComponent(_store.Value);
            base.Dispose();
        }
    }
}
