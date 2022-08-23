namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.ViewModels;
    using System;

    public abstract class TraitViewModel : ViewModelBase, ITraitViewModel
    {
        public TraitViewModel(Trait trait)
        {
            Id = trait.Id;
            _name = trait.Name;
            _description = trait.Description;
            _notes = trait.Notes;
            _source = trait.Source;
            _page = trait.Page;
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

        protected void Update(Trait record)
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
