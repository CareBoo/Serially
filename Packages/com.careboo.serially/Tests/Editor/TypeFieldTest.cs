using UnityEngine;
using NUnit.Framework;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using UnityEngine.TestTools;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace CareBoo.Serially.Editor.Tests
{
    public class TypeFieldTest
    {
        [Serializable]
        public class A { }

        [Serializable]
        public class B : A { }

        [Serializable, ProvideSourceInfo]
        public class C : A { }

        [Test]
        public void ClickingTheTypeFieldOfTypeWithProvideSourceInfoShouldPingTheMonoScript()
        {
            var clickEvent = new Event() { clickCount = 1 };
            var type = typeof(C);
            var monoScript = HandleTypeLabelClicked(clickEvent, type);
            Assert.IsNotNull(monoScript);
        }

        [Test]
        public void ClickingTheTypeFieldOfTypeWithoutProvideSourceInfoShouldLogWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(nameof(ProvideSourceInfoAttribute)));
            var clickEvent = new Event() { clickCount = 1 };
            var type = typeof(B);
            var monoScript = HandleTypeLabelClicked(clickEvent, type);
            Assert.IsNull(monoScript);
        }

        [UnityTest]
        public IEnumerator ClickingTypePickerOpensTypePickerWindow()
        {
            yield return new OnGUITest(() =>
            {
                var position = new Rect(x: 0, y: 0, width: 150, height: 30);
                var clickerArea = GetPickerArea(position);
                Event.current = new Event()
                {
                    clickCount = 1,
                    type = EventType.MouseDown,
                    mousePosition = clickerArea.center
                };
                var type = typeof(A);
                var types = new[] { typeof(A), typeof(B), typeof(C) };
                Action<Type> onSelect = _ => { return; };
                TypeField(position, type, types, onSelect);
                bool isWindowOpen = UnityObject.FindObjectsOfType<TypePickerWindow>().Length > 0;
                Assert.IsTrue(isWindowOpen);
            });
        }
    }
}
