using UnityEngine;
using NUnit.Framework;
using static CareBoo.Serially.Editor.EditorGUIExtensions;

namespace CareBoo.Serially.Editor.Tests
{
    public class TypeFieldTest
    {
        [ProvideSourceInfo]
        public class TestClass { }

        [Test]
        public void ClickingTheTypeFieldShouldPingTheMonoScript()
        {
            var clickEvent = new Event() { clickCount = 1 };
            var type = typeof(TestClass);
            var monoScript = HandleTypeLabelClicked(clickEvent, type);
            Assert.IsNotNull(monoScript);
        }
    }
}
