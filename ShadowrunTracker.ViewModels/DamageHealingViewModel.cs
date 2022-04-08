using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public class DamageHealingViewModel : ModalViewModelBase<IDamageHealingViewModel>, IDamageHealingViewModel, IDisposable
    {
        private bool _open = true;

        public ICharacterViewModel Character { get; }

        private bool m_IsHealing;
        public bool IsHealing
        {
            get => m_IsHealing;
            set => this.RaiseAndSetIfChanged(ref m_IsHealing, value);
        }

        public int Physical { get; set; }
        public int Stun { get; set; }

        public DamageHealingViewModel(ICharacterViewModel character)
            : base()
        {
            Character = character;
        }

        protected override void OnOk()
        {
            if (_open)
            {
                _open = false;
                m_Complete.OnNext(this);
                m_Complete.OnCompleted();
            }
        }

        protected override void OnCancel()
        {
            if (_open)
            {
                _open = false;
                Physical = 0;
                Stun = 0;
                m_Complete.OnCompleted();
            }
        }
    }
}
