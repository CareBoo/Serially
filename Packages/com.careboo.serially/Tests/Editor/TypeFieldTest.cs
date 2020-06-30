using UnityEngine;
using NUnit.Framework;
using static CareBoo.Serially.Editor.EditorGUIExtensions;

namespace CareBoo.Serially.Editor.Tests
{
    [ProvideSourceInfo]
    public class TestClass { }

    public class TypeFieldTest
    {
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
