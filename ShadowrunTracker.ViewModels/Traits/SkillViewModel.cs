namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class SkillViewModel : LeveledTraitViewModel, ISkillViewModel
    {
        protected static readonly ISet<string> _skillRecordProperties;

        static SkillViewModel()
        {
            _skillRecordProperties = new HashSet<string>
            {
                nameof(LinkedAttribute)
            };
            _skillRecordProperties.UnionWith(_leveledTraitRecordProperties);
        }

        public SkillViewModel(Skill record)
            : base(record)
        {
            _linkedAttribute = record.LinkedAttribute;
            PropertyChanged += OnPropertyChanged;
        }

        private SR5Attribute _linkedAttribute;
        public SR5Attribute LinkedAttribute
        {
            get => _linkedAttribute;
            set => this.RaiseAndSetIfChanged(ref _linkedAttribute, value);
        }

        RecordBase IRecordViewModel.Record => ToRecord();

        public Skill Record => ToRecord();

        public Skill ToRecord()
        {
            return this.ToModel();
        }

        public void Update(Skill record)
        {
            base.Update(record);
            LinkedAttribute = record.LinkedAttribute;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PushUpdate && ReferenceEquals(this, sender) && _traitRecordProperties.Contains(e.PropertyName))
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

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
