using ReactiveUI;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public class DelayActionViewModel : ModalViewModelBase<IDelayActionViewModel>, IDelayActionViewModel
    {
        private static IReadOnlyList<DelayAction> _delayActions = new List<DelayAction>
        {
            new DelayAction { Name = "Block", Value = -5 },
            new DelayAction { Name = "Dodge", Value = -5 },
            new DelayAction { Name = "Hit the dirt", Value = -5 },
            new DelayAction { Name = "Intercept", Value = -5 },
            new DelayAction { Name = "Parry", Value = -5 },
            new DelayAction { Name = "Full defense", Value = -10 },
            new DelayAction { Name = "Surpice", Value = -10 },
            new DelayAction { Name = "Called shot - Shake up", Value = -5 },
            new DelayAction { Name = "Full Matrix Defense", Value = -10 },
            new DelayAction { Name = "Spell defense", Value = -5 },
        };

        private bool _open = true;


        public IParticipantInitiativeViewModel Participant { get; }
        public IReadOnlyCollection<DelayAction> Actions => _delayActions;

        private DelayAction? m_CurrentAction;
        public DelayAction? CurrentAction
        {
            get => m_CurrentAction;
            set
            {
                if (value != null)
                {
                    m_CurrentAction = value;
                    OnOk();
                }
            }
        }

        public DelayActionViewModel(IParticipantInitiativeViewModel participant)
        {
            Participant = participant;
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
                CurrentAction = null;
                m_Complete.OnCompleted();
            }
        }
    }
}
