namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;

    public class SelectableViewModel<T> : ViewModelBase, ISelectableViewModel<T>
    {
        private T? m_ViewModel;
        public T? ViewModel
        {
            get => m_ViewModel;
            set => this.RaiseAndSetIfChanged(ref m_ViewModel, value);
        }

        private bool m_IsSelected;
        public bool IsSelected
        {
            get => m_IsSelected;
            set => this.RaiseAndSetIfChanged(ref m_IsSelected, value);
        }

        public SelectableViewModel(T viewModel)
        {
            m_ViewModel = viewModel;
        }
    }
}
