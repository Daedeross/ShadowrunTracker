namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ImprovementViewModel : ViewModelBase, IImprovementViewModel
    {
        protected static readonly ISet<string> RecordProperties = new HashSet<string>
        {
            nameof(Name),
            nameof(TargetKind),
            nameof(Target),
            nameof(Value),
        };

        private bool _pushUpdate = true;

        public ImprovementViewModel(Improvement record)
        {
            Id = record.Id == Guid.Empty ? Guid.NewGuid() : record.Id;
            m_Name = record.Name;
            m_TargetKind = record.TargetKind;
            m_Target = record.Target;
            m_Value = record.Value;

            PropertyChanged += OnPropertyChanged;
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

        public Guid Id { get; protected set; }


        #region IRecordViewModel implementation

        RecordBase IRecordViewModel.Record => ToRecord();

        public Improvement Record => ToRecord();

        public Improvement ToRecord()
        {
            return this.ToModel();
        }

        public void Update(Improvement record)
        {
            try
            {
                _pushUpdate = false;

                Name = record.Name;
                TargetKind = record.TargetKind;
                Target = record.Target;
                Value = record.Value;
            }
            finally
            {
                _pushUpdate = true;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pushUpdate && ReferenceEquals(this, sender) && RecordProperties.Contains(e.PropertyName))
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

        #endregion

        private bool disposedValue;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PropertyChanged -= OnPropertyChanged;
                }

                disposedValue = true;
            }
            base.Dispose(disposing);
        }
    }
}
