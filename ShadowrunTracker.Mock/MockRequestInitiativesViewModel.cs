using DynamicData.Binding;
using ShadowrunTracker.Mock.TestData;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ShadowrunTracker.Mock
{
    public class MockRequestInitiativesViewModel : RequestInitiativesViewModel
    {
        public MockRequestInitiativesViewModel()
            : base(MockViewModelFactory.Instance, new ObservableCollectionExtended<ICharacterViewModel>(TestCharacters.TestGroup))
        {
        }
    }
}
