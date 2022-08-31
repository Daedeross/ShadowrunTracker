namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.ViewModels;
    using System;
    using System.Collections.Generic;

    public abstract class TraitViewModel : ViewModelBase, ITraitViewModel
    {
        protected static readonly ISet<string> _traitRecordProperties = new HashSet<string>
        {
            nameof(Name),
            nameof(Description),
            nameof(Notes),
            nameof(Source),
            nameof(Page),
        };

        protected bool PushUpdate = true;

        public TraitViewModel(Trait record)
        {
            Id = record.Id;
            _name = record.Name;
            _description = record.Description;
            _notes = record.Notes;
            _source = record.Source;
            _page = record.Page;
        }

        public Guid Id { get; protected set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => this.RaiseAndSetIfChanged(ref _notes, value);
        }

        private string _source;

        public string Source
        {
            get => _source;
            set => this.RaiseAndSetIfChanged(ref _source, value);
        }

        private int _page;
        public int Page
        {
            get => _page;
            set => this.RaiseAndSetIfChanged(ref _page, value);
        }

        public void Update(Trait record)
        {
            try
            {
                PushUpdate = false;
                DoUpdate(record);
            }
            finally
            {
                PushUpdate = true;
            }
        }

        protected void DoUpdate(Trait record)
        {
            if (Id != record.Id)
            {
                throw new InvalidOperationException("Ids do not match");
            }
            Name = record.Name;
            Description = record.Description;
            Notes = record.Notes;
            Source = record.Source;
            Page = record.Page;
        }
    }
}
