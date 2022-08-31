// // ********************************************************************************************************
// //
// // Origin: https://github.com/patrickgehrke/ReactiveUi-CastleWindsor-Adapter
// // Solution: ReactiveUiCastleWindsorAdapter
// // Project:  ReactiveUiCastleWindsorAdapter
// // Filename: CastleWindsorDependencyResolver.cs
// //
// // Author:   Gehrke, Patrick
// // Created:  02.11.2019 11:57
// // Updated:  02.11.2019 11:58
// //
// // MIT License
// // Copyright(c) 2019 Patrick Gehrke
// // 
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// // 
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.
// // ********************************************************************************************************

#nullable disable

namespace ShadowrunTracker.Wpf.Configuration
{
    using System;
    using System.Collections.Generic;
    using Splat;
    using Castle.Windsor;
    using Castle.MicroKernel.Registration;

    public class CastleWindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _windsorContainer;

        public CastleWindsorDependencyResolver(IWindsorContainer windsorContainer)
        {
            _windsorContainer = windsorContainer;
        }

        public object GetService(Type serviceType, string contract = null)
        {
            return string.IsNullOrEmpty(contract) ? _windsorContainer.Resolve(serviceType) : _windsorContainer.Resolve(contract, serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return (IEnumerable<object>)_windsorContainer.ResolveAll(serviceType);
        }

        public bool HasRegistration(Type serviceType, string contract = null)
        {
            return _windsorContainer.Kernel.HasComponent(serviceType);
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            _windsorContainer.Register(Component.For(serviceType)
                .Instance(factory())
                .IsDefault()
                .Named(Guid.NewGuid().ToString()));
        }

        #region not needed

        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
