using System;
using System.Linq.Expressions;
using System.Reflection;
using static CareBoo.Serially.Editor.Reflection.ReflectionUtil;
using NUnit.Framework;

namespace CareBoo.Serially.Editor.Tests
{
    class TypeFilterAttributeTest
    {
        class TestClass
        {
            [TypeFilter(derivedFrom: typeof(TestClass))]
            public SerializableType TypeDerivedFromTestClass;

            [TypeFilter(filterName: nameof(Filter))]
            public SerializableType FilteredType;

            public bool IsFiltering { get; set; }

            public Func<Type, bool> FilterDelegate => Filter;

            public bool Filter(Type _) => IsFiltering;
        }

        [Test]
        public void SettingDerivedFromShouldSetFilterToDerivedFromFilter()
        {
            var field = GetFieldInfo<TestClass>(x => x.TypeDerivedFromTestClass);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var expected = attribute.DerivedFromFilter;
            var actual = attribute.Filter;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetDerivedFromShouldEqualDerivedFrom()
        {

            var field = GetFieldInfo<TestClass>(x => x.TypeDerivedFromTestClass);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var expected = typeof(TestClass);
            var actual = attribute.DerivedFrom;
            Assert.AreEqual(expected, actual);
        }

        [Test]
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
