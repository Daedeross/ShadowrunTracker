namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;

    public class DamageBoxViewModel : ReactiveObject, IDamageBoxViewModel
    {
        public bool IsFilled { get; init; }

        private bool m_IsHovered;
        public bool IsHovered
        {
            get => m_IsHovered;
            set => this.RaiseAndSetIfChanged(ref m_IsHovered, value);
        }

        private bool m_ShouldHighlight;
        public bool ShouldHighlight
        {
            get => m_ShouldHighlight;
            set => this.RaiseAndSetIfChanged(ref m_ShouldHighlight, value);
        }

    }
}
