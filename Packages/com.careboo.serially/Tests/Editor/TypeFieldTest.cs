using UnityEngine;
using NUnit.Framework;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using UnityEngine.TestTools;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using UnityEditor;

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

        [TearDown]
        public void CloseTestWindow()
        {
            if (EditorWindow.HasOpenInstances<TestEditorWindow>())
                EditorWindow.GetWindow<TestEditorWindow>().Close();
        }

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
            bool onGuiCalled = false;
            var position = new Rect(x: 0, y: 0, width: 150, height: 30);
            var pickerArea = GetPickerArea(position);
            void OnGUI()
            {
                var type = typeof(A);
                var types = new[] { typeof(A), typeof(B), typeof(C) };
                var evt = SimulateMouseDown(1, pickerArea.center);
                TypeField(position, type, types, null, evt);
                onGuiCalled = true;
            }
            var testWindow = TestEditorWindow.ShowWindow();
            testWindow.onGui = OnGUI;
            yield return new WaitUntil(() => onGuiCalled);
            Assert.IsTrue(EditorWindow.HasOpenInstances<TypePickerWindow>());
            EditorWindow.GetWindow<TypePickerWindow>().Close();
        }

        private Event SimulateMouseDown(int clickCount, Vector2 position)
        {
            return new Event()
            {
                type = EventType.MouseDown,
                clickCount = clickCount,
                mousePosition = position
            };
        }
    }
}
