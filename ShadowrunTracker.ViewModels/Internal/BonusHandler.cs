namespace ShadowrunTracker.ViewModels.Internal
{
    using ShadowrunTracker.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal class BonusHandler : IDisposable
    {
        private readonly IImprovementViewModel _improvement;
        private readonly ICharacterViewModel _character;
        private Func<int> _getter;
        private Action<int> _setter;

        private int _cachedValue = 0;

        public BonusHandler(ICharacterViewModel character, IImprovementViewModel improvement)
        {
            _character = character ?? throw new ArgumentNullException(nameof(character));
            _improvement = improvement ?? throw new ArgumentNullException(nameof(improvement));

            if (improvement.TargetKind == Model.TraitKind.None)
            {
                throw new ArgumentOutOfRangeException(nameof(improvement.TargetKind));
            }

            _improvement.PropertyChanged += OnImprovementChanged;
            (_getter, _setter) = GetTarget();
            RecalcBonus();
        }

        private (Func<int> getter, Action<int> setter) GetTarget()
        {
            return _improvement.TargetKind switch
            {
                Model.TraitKind.Attribute => GetAttribute(),
                Model.TraitKind.Skill => GetSkill(),
                _ => throw new InvalidOperationException("Unknown target kind."),
            };
        }

        private (Func<int> getter, Action<int> setter) GetAttribute()
        {
            var name = $"Bonus{_improvement.Target}";
            var getter = LambdaMaker.GetGetterWithClosure<ICharacterViewModel, int>(_character, name);
            var setter = LambdaMaker.GetSetterWithClosure<ICharacterViewModel, int>(_character, name);

            return (getter, setter);
        }

        private (Func<int> getter, Action<int> setter) GetSkill()
        {
            var skill = _character.Skills.FirstOrDefault(s => s.Name == _improvement.Name);
            if (skill is null)
            {
                throw new KeyNotFoundException($"Target skill {_improvement.Name} not found on character.");
            }
            var getter = LambdaMaker.GetGetterWithClosure<ISkillViewModel, int>(skill, nameof(ISkillViewModel.BonusRating));
            var setter = LambdaMaker.GetSetterWithClosure<ISkillViewModel, int>(skill, nameof(ISkillViewModel.BonusRating));

            return (getter, setter);
        }

        private void OnImprovementChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IImprovementViewModel.Value))
            {
                RecalcBonus();
            }
            if (e.PropertyName == nameof(IImprovementViewModel.TargetKind) || e.PropertyName == nameof(IImprovementViewModel.Target))
            {
                (_getter, _setter) = GetTarget();
            }
        }

        private void RecalcBonus()
        {
            if (_cachedValue != _improvement.Value)
            {
                var newValue = _getter() - _cachedValue + _improvement.Value;
                _setter(newValue);
                _cachedValue = newValue;
            }
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _improvement.PropertyChanged -= OnImprovementChanged;
                    _setter(-_improvement.Value);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
        }
    }
}
