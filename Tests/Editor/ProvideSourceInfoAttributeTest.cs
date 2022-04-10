using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace CareBoo.Serially.Editor.Tests
{
    public class ProvideSourceInfoAttributeTest
    {
        [ProvideSourceInfo]
        public class TestClass { }

        [ProvideSourceInfo]
        public string MyProperty { get; }

        private ProvideSourceInfoAttribute GetAttribute(MemberInfo element)
        {
            var attr = Attribute.GetCustomAttribute(element, typeof(ProvideSourceInfoAttribute));
            return (ProvideSourceInfoAttribute)attr;
        }

        private MemberInfo GetMemberInfo<TMember>(Expression<Func<ProvideSourceInfoAttributeTest, TMember>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return memberExpression.Member;
        }

        [Test]
        [Timeout(5000)]
        public void SettingAttributeShouldSetLineNumber()
        {
            var attribute = GetAttribute(typeof(TestClass));
            Assert.AreEqual(10, attribute.LineNumber);
        }

        [Test]
        [Timeout(5000)]
        public void SettingAttributeOnClassShouldSetMemberNameToEmpty()
        {
            var attribute = GetAttribute(typeof(TestClass));
            Assert.AreEqual(string.Empty, attribute.Member);
        }

        [Test]
        [Timeout(5000)]
        public void SettingAttributeOnPropertyShouldSetMemberNameToPropertyName()
        {
            var attribute = GetAttribute(GetMemberInfo(x => x.MyProperty));
            Assert.AreEqual(nameof(MyProperty), attribute.Member);
        }

        [Test]
        [Timeout(5000)]
        public void SettingAttributeShouldSetAssetPath()
        {
            var attribute = GetAttribute(typeof(TestClass));
            var thisFileName = $"{nameof(ProvideSourceInfoAttributeTest)}.cs";
            Assert.IsTrue(attribute.AssetPath.Contains(thisFileName));
        }
    }
}
