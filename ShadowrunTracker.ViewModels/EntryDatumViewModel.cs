namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ReactiveUI.Validation.Contexts;
    using ReactiveUI.Validation.Extensions;
    using ShadowrunTracker.Model;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;


    public class EntryDatumViewModel : ViewModelBase, IEntryDatumViewModel
    {
        private readonly Type _type;
        private readonly TypeConverter _converter;

        public EntryDatumViewModel(EntryDatum options)
        {
            Name = options.Name;

            _type = options.Type ?? typeof(string);

            _converter = TypeDescriptor.GetConverter(_type);
            if (_type != typeof(string))
            {
                if (!_converter.CanConvertFrom(typeof(string)))
                {
                    throw new InvalidCastException($"No converter found to parse type {_type}");
                }
            }

            var ok = TryConvert(Text, out _value);

            this.ValidationRule(
                vm => vm.Text,
                text => TryConvert(text, out _value),
                $"Cannot parse text to {_type}");

            ValidationContext.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(IsValid))
                {
                    this.RaisePropertyChanged(nameof(IsValid));
                }
            };
        }

        public string Name { get; private set; }

        private string m_Text = string.Empty;
        public string Text
        {
            get => m_Text;
            set => this.RaiseAndSetIfChanged(ref m_Text, value);
        }

        private object? _value;
        public object? Value => _value;

        public bool IsValid => ValidationContext.IsValid;

        public ValidationContext ValidationContext { get; } = new ValidationContext();

        private bool TryConvert(string? text, out object? value)
        {
            try
            {
                if (_type == typeof(string))
                {
                    value = text;
                }
                else
                {
                    value = _converter.ConvertFromString(text);
                }
                return true;
            }
            catch (NotSupportedException)
            {
                value = default;
                return false;
            }
        }
    }
}
