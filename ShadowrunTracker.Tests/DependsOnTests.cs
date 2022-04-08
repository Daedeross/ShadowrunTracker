using ReactiveUI;
using ShadowrunTracker.Model;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
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

        public class TestClassWithCache : ReactiveObject, IViewModel, IDisposable
        {
            private readonly ChangeNotificationHandler<TestClassWithCache> _changeNotificationHandler;

            public TestClassWithCache()
            {
                _changeNotificationHandler = new ChangeNotificationHandler<TestClassWithCache>(this);
            }

            private int m_Prop1;
            public int Prop1
            {
                get => m_Prop1;
                set => this.RaiseAndSetIfChanged(ref m_Prop1, value);
            }

            [DependsOn(nameof(Prop1))]
            public int Prop2 => m_Prop1 + 1;

            [DependsOn(nameof(Prop2))]
            public int Prop3 => Prop2 + 1;

            [DependsOn(nameof(Prop1))]
            public int Prop4 => Convert.ToInt32(Math.Ceiling(Prop1 * 0.5f));

            public void Dispose()
            {
                _changeNotificationHandler.Dispose();
            }
        }

        [Fact]
        public void DependsOnFiresEventTest()
        {
            var obj = new TestClass
            {
                Prop1 = 1,
            };

            Assert.PropertyChanged(obj, nameof(TestClass.Prop1), () => obj.Prop1 = 2);

            obj.Prop1 = 1;
            Assert.PropertyChanged(obj, nameof(TestClass.Prop2), () => obj.Prop1 = 2);
        }

        [Fact]
        public void DependsOnFiresEventsIfChanged()
        {
            var obj = new TestClassWithCache
            {
                Prop1 = 1,
            };

            Assert.PropertyChanged(obj, nameof(TestClassWithCache.Prop1), () => obj.Prop1 = 2);

            obj.Prop1 = 1;
            Assert.PropertyChanged(obj, nameof(TestClassWithCache.Prop2), () => obj.Prop1 = 2);

            obj.Prop1 = 1;
            Assert.PropertyChanged(obj, nameof(TestClassWithCache.Prop3), () => obj.Prop1 = 2);

            bool fired = false;

            obj.Prop1 = 1;
            PropertyChangedEventHandler handler = (sender, e) => { if (e.PropertyName == nameof(TestClassWithCache.Prop4)) { fired = true; } };

            obj.PropertyChanged += handler;

            obj.Prop1 = 2;

            Assert.False(fired);

            obj.PropertyChanged -= handler;

            Assert.PropertyChanged(obj, nameof(TestClassWithCache.Prop4), () => obj.Prop1 += 2);
        }
    }
}
