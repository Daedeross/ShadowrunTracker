namespace ShadowrunTracker.Wpf
{
    using ShadowrunTracker.ViewModels;
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    public interface IDrawerManager: INotifyPropertyChanged
    {
        bool LeftDrawerVisible { get; }
        bool RightDrawerVisible { get; }
        bool TopDrawerVisible { get; }
        bool BottomDrawerVisible { get; set; }

        IModalViewModel? LeftDrawerContext { get; }
        IModalViewModel? RightDrawerContext { get; }
        IModalViewModel? TopDrawerContext { get; }
        IModalViewModel? BottomDrawerContext { get; }

        IObservable<TResult> ShowLeftModal<TViewModel, TResult>( TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel;

        IObservable<TResult> ShowRightModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel;

        IObservable<TResult> ShowTopModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel;

        IObservable<TResult> ShowBottomModal<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, IObservable<TResult>> resultFactory)
            where TViewModel : IModalViewModel;
        
        void CancelDrawer(Dock dock);

        void CancelLeftDrawer();
        void CancelRightDrawer();
        void CancelTopDrawer();
        void CancelBottomDrawer();
    }
}
