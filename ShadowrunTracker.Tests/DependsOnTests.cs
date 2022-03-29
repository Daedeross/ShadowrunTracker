using ReactiveUI;
using ShadowrunTools.Model;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System.ComponentModel;
using Xunit;

namespace ShadowrunTracker.Tests
{
    public class DependsOnTests
    {

        public class TestClass : ReactiveObject, IViewModel
        {
            private int m_Prop1;
            public int Prop1
            {
                get => m_Prop1;
                set => this.SetAndRaiseIfChanged(ref m_Prop1, value);
            }

            [DependsOn(nameof(Prop1))]
            public int Prop2 => m_Prop1 + 1;
        }

        [Fact]
        public void DependsOnFiresEventTest()
        {
            var obj = new TestClass
            {
                Prop1 = 1,
            };

            obj.PropertyChanged += OnPropertyChanged;

            obj.Prop1 = 2;

            obj.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ;
        }
    }
}
