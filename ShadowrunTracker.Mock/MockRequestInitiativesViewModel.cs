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
            : base(TestData.TestCharacters.DataStore, new ObservableCollectionExtended<ICharacterViewModel>(TestCharacters.TestGroup))
        {
        }
    }
}
