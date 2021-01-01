using System;
using System.Reflection;
using static CareBoo.Serially.Editor.Tests.Reflection.ReflectionUtil;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CareBoo.Serially.Editor.Tests
{
    class TypeFilterAttributeTest
    {
        class TestClass
        {
            [TypeFilter(derivedFrom: typeof(TestClass))]
            public SerializableType TypeDerivedFromTestClass = null;

            [TypeFilter(filterName: nameof(Filter))]
            public SerializableType FilteredType = null;

            public bool IsFiltering { get; set; }

            public Func<IEnumerable<Type>, IEnumerable<Type>> FilterDelegate => Filter;

            public IEnumerable<Type> Filter(IEnumerable<Type> sequence)
            {
                return sequence.Where(_ => IsFiltering);
            }
        }

        [Test]
        [Timeout(5000)]
        public void SettingDerivedFromShouldSetFilterToDerivedFromFilter()
        {
            var field = GetFieldInfo<TestClass>(x => x.TypeDerivedFromTestClass);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var expected = attribute.DerivedFromFilter;
            var actual = attribute.Filter;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Timeout(5000)]
        public void SetDerivedFromShouldEqualDerivedFrom()
        {

            var field = GetFieldInfo<TestClass>(x => x.TypeDerivedFromTestClass);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var expected = typeof(TestClass);
            var actual = attribute.DerivedFrom;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Timeout(5000)]
        public void SettingFilterNameToMethodShouldEqualFilter()
        {
            var instance = new TestClass();
            var field = GetFieldInfo<TestClass>(x => x.FilteredType);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var expected = instance.FilterDelegate;
            var actual = attribute.GetFilter(instance);
            Assert.AreEqual(expected, actual);
        }
    }
}
