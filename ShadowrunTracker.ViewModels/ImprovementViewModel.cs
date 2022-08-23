namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System;

    public class ImprovementViewModel : ReactiveObject, IImprovementViewModel
    {
        public ImprovementViewModel(Improvement improvement)
        {
            m_Name = improvement.Name;
            m_TargetKind = improvement.TargetKind;
            m_Target = improvement.Target;
            m_Value = improvement.Value;
        }

        private string m_Name;
        public string Name
        {
            get => m_Name;
            set => this.RaiseAndSetIfChanged(ref m_Name, value);
        }

        private TraitKind m_TargetKind;
        public TraitKind TargetKind
        {
            get => m_TargetKind;
            set => this.RaiseAndSetIfChanged(ref m_TargetKind, value);
        }

        private string m_Target;
        public string Target
        {
            get => m_Target;
            set => this.RaiseAndSetIfChanged(ref m_Target, value);
        }

        private int m_Value;
        public int Value
        {
            get => m_Value;
            set => this.RaiseAndSetIfChanged(ref m_Value, value);
        }

        public Guid Id => throw new NotImplementedException();

        public Improvement ToRecord()
        {
            return this.ToModel();
        }

        public void Update(Improvement record)
        {
            Name = record.Name;
            TargetKind = record.TargetKind;
            Target = record.Target;
            Value = record.Value;
        }
    }
}
