// // ********************************************************************************************************
// //
// // Origin: https://github.com/patrickgehrke/ReactiveUi-CastleWindsor-Adapter
// // Solution: ReactiveUiCastleWindsorAdapter
// // Project:  ReactiveUiCastleWindsorAdapter
// // Filename: Bootstrapper.cs
// //
// // Author:   Gehrke, Patrick
// // Created:  02.11.2019 11:02
// // Updated:  02.11.2019 11:03
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

namespace ShadowrunTracker.Wpf
{
    using Castle.Windsor;
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using ShadowrunTracker.Wpf.Configuration;
    using Splat;
    using System;

    public class Bootstrapper : IDisposable
    {
        private readonly IWindsorContainer _container;

        public Bootstrapper()
        {
            _container = new WindsorContainer();
        }

        public Bootstrapper Setup()
        {
            Locator.SetLocator(new CastleWindsorDependencyResolver(_container));
            _container.Install(new ApplicationInstaller());
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();
            Locator.CurrentMutable.RegisterLazySingleton(() => _container.Resolve<IViewLocator>(nameof(WindsorViewLocator)), typeof(IViewLocator));
            return this;
        }

        public void DisplayRootView()
        {
            var rootView = _container.Resolve<IViewFor<IWorkspaceViewModel>>() as System.Windows.Window;
            rootView.Show();
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}