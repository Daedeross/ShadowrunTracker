namespace ShadowrunTracker.Wpf.Helpers
{
    using ReactiveUI;
    using ShadowrunTracker.ViewModels;
    using System;
    using System.Collections.Concurrent;
    using System.Reactive.Linq;
    using System.Windows.Controls;

    public class DrawerManager : ReactiveObject, IDrawerManager
    {
        private readonly ConcurrentDictionary<Dock, ViewModels.ICancelable> _dockContexts = new();

        public DrawerManager()
        {

        }

        private bool m_LeftDrawerVisible;
        public bool LeftDrawerVisible
        {
            get => m_LeftDrawerVisible;
            set => this.RaiseAndSetIfChanged(ref m_LeftDrawerVisible, value);
        }

        private bool m_RightDrawerVisible;
        public bool RightDrawerVisible
        {
            get => m_RightDrawerVisible;
            set => this.RaiseAndSetIfChanged(ref m_RightDrawerVisible, value);
        }
        private bool m_TopDrawerVisible;
        public bool TopDrawerVisible
        {
            get => m_TopDrawerVisible;
            set => this.RaiseAndSetIfChanged(ref m_TopDrawerVisible, value);
        }

        private bool m_BottomDrawerVisible;
        public bool BottomDrawerVisible
        {
            get => m_BottomDrawerVisible;
            set => this.RaiseAndSetIfChanged(ref m_BottomDrawerVisible, value);
        }

        private IModalViewModel? m_LeftDrawerContext;
        public IModalViewModel? LeftDrawerContext
        {
            get => m_LeftDrawerContext;
            set => this.RaiseAndSetIfChanged(ref m_LeftDrawerContext, value);
        }

        private IModalViewModel? m_RightDrawerContext;
        public IModalViewModel? RightDrawerContext
        {
            get => m_RightDrawerContext;
            set => this.RaiseAndSetIfChanged(ref m_RightDrawerContext, value);
        }

        private IModalViewModel? m_TopDrawerContext;
        public IModalViewModel? TopDrawerContext
        {
            get => m_TopDrawerContext;
            set => this.RaiseAndSetIfChanged(ref m_TopDrawerContext, value);
        }

        private IModalViewModel? m_BottomDrawerContext;
        public IModalViewModel? BottomDrawerContext
        {
            get => m_BottomDrawerContext;
            set => this.RaiseAndSetIfChanged(ref m_BottomDrawerContext, value);
        }

        public IObservable<TResult> ShowLeftModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel
        {
            _dockContexts.AddOrUpdate(Dock.Left, viewModel, ReplaceViewModel(viewModel));

            LeftDrawerContext = viewModel;
            var result = resultFactory(viewModel);
            result.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(r => { }, () => LeftDrawerVisible = false);
            LeftDrawerVisible = true;

            return result;
        }

        public IObservable<TResult> ShowRightModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel
        {
            _dockContexts.AddOrUpdate(Dock.Right, viewModel, ReplaceViewModel(viewModel));

            RightDrawerContext = viewModel;
            var result = resultFactory(viewModel);
            result.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(r => { }, () => RightDrawerVisible = false);
            RightDrawerVisible = true;

            return result;
        }

        public IObservable<TResult> ShowTopModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel
        {
            _dockContexts.AddOrUpdate(Dock.Top, viewModel, ReplaceViewModel(viewModel));

            TopDrawerContext = viewModel;
            var result = resultFactory(viewModel);
            result.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(r => { }, () => TopDrawerVisible = false);
            TopDrawerVisible = true;

            return result;
        }

        public IObservable<TResult> ShowBottomModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel
        {
            _dockContexts.AddOrUpdate(Dock.Bottom, viewModel, ReplaceViewModel(viewModel));

            BottomDrawerContext = viewModel;
            var result = resultFactory(viewModel);
            result.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(r => { }, () => BottomDrawerVisible = false);
            BottomDrawerVisible = true;
            this.RaisePropertyChanged(nameof(BottomDrawerVisible));

            return result;
        }

        private Func<Dock, ICancelable, ICancelable> ReplaceViewModel<TViewModel>(TViewModel newVM)
            where TViewModel : ICancelable
        {

            return new Func<Dock, ICancelable, ICancelable>( (dock, oldVM) =>
            {
                oldVM.Cancel();
                if (typeof(IDisposableModalViewModel<>).IsAssignableFrom(oldVM.GetType()))
                {
                    (oldVM as IDisposable)?.Dispose();
                }

                return newVM;
            });
        }

        public void CancelDrawer(Dock dock)
        {
            switch (dock)
            {
                case Dock.Left:
                    CancelLeftDrawer();
                    break;
                case Dock.Top:
                    CancelTopDrawer();
                    break;
                case Dock.Right:
                    CancelRightDrawer();
                    break;
                case Dock.Bottom:
                    CancelBottomDrawer();
                    break;
                default:
                    break;
            }
        }

        public void CancelLeftDrawer()
        {
            CancelModal(LeftDrawerContext);
            LeftDrawerVisible = false;
            LeftDrawerContext = null;
        }

        public void CancelRightDrawer()
        {
            CancelModal(RightDrawerContext);
            RightDrawerVisible = false;
            RightDrawerContext = null;
        }

        public void CancelTopDrawer()
        {
            CancelModal(TopDrawerContext);
            TopDrawerVisible = false;
            TopDrawerContext = null;
        }

        public void CancelBottomDrawer()
        {
            CancelModal(BottomDrawerContext);
            BottomDrawerVisible = false;
            BottomDrawerContext = null;
        }

        public void CancelModal(IModalViewModel? vm)
        {
            if (vm is null)
            {
                return;
            }

            vm.Cancel();
            if (typeof(IDisposableModalViewModel<>).IsAssignableFrom(vm.GetType()))
            {
                vm.Dispose();
            }
        }
    }
}
