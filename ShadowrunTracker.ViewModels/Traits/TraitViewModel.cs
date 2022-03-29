using ReactiveUI;
using ShadowrunTracker.Data;
using ShadowrunTracker.ViewModels;

namespace ShadowrunTracker.ViewModels.Traits
{
    public class TraitViewModel : ReactiveObject, ITraitViewModel
    {
        public TraitViewModel(ITrait trait)
        {
            _name = trait.Name;
            _description = trait.Description;
            _notes = trait.Notes;
            _source = trait.Source;
            _page = trait.Page;
        }

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
    }
}
