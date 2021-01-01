using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;

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

        private static bool thisScriptOpenedAtC = false;

        private static Rect TypeFieldRect => new Rect(x: 0, y: 0, width: 150, height: 18);
        private static Lazy<IEnumerable<Type>> Types =>
            new Lazy<IEnumerable<Type>>(() => new[] { typeof(A), typeof(B), typeof(C) });

        [TearDown]
        public void CloseTestWindow()
        {
            if (EditorWindow.HasOpenInstances<TestEditorWindow>())
                EditorWindow.GetWindow<TestEditorWindow>().Close();
        }

        [Test]
        [Timeout(5000)]
        public void ClickingTheTypeFieldOfTypeWithProvideSourceInfoShouldPingTheMonoScript()
        {
            var guiEvent = new GuiEvent(EventType.MouseDown, Vector2.zero, 1, 0);
            var typeField = new TypeField(Rect.zero, typeof(C), null, null, guiEvent);
            var monoScript = typeField.HandleTypeLabelClicked();
            Assert.IsNotNull(monoScript);
        }

        [Test]
        [Timeout(5000)]
        public void ClickingTheTypeFieldOfTypeWithoutProvideSourceInfoShouldLogWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(nameof(ProvideSourceInfoAttribute)));
            var guiEvent = new GuiEvent(EventType.MouseDown, Vector2.zero, 1, 0);
            var typeField = new TypeField(Rect.zero, typeof(B), null, null, guiEvent);
            var monoScript = typeField.HandleTypeLabelClicked();
            Assert.IsNull(monoScript);
        }

        [UnityTest]
        [Timeout(5000)]
        public IEnumerator ClickingTypePickerShouldOpenTypePickerWindow()
        {
            var pickerArea = TypeField.GetPickerButtonArea(TypeFieldRect);
            var guiEvent = new GuiEvent(
                EventType.MouseDown,
                pickerArea.center,
                1,
                0
                );
            var typeField = new TypeField(TypeFieldRect, typeof(A), Types, null, guiEvent);
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(typeField.DrawGui);
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            Assert.IsTrue(EditorWindow.HasOpenInstances<TypePickerWindow>());
            EditorWindow.GetWindow<TypePickerWindow>().Close();
        }

        [UnityTest]
        [Timeout(5000)]
        public IEnumerator ClickingTypeLabelTwiceForTypeDefinedInThisAssetShouldOpenThisAsset()
        {
            var guiEvent = new GuiEvent(
                EventType.MouseDown,
                TypeFieldRect.center,
                2,
                0
                );
            var typeField = new TypeField(
                TypeFieldRect,
                typeof(C),
                Types,
                null,
                guiEvent
                );
            thisScriptOpenedAtC = false;
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(typeField.DrawGui);
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            Assert.IsTrue(thisScriptOpenedAtC);
        }

        [UnityTest]
        [Timeout(5000)]
        public IEnumerator TypeFieldShouldShowWithoutErrors()
        {
            var type = typeof(A);
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => new TypeField(TypeFieldRect, type, Types, null).DrawGui());
            yield return new WaitUntil(testWindow.OnGUIInitialized);
        }

        [OnOpenAsset(0)]
        public static bool CheckForThisScriptOpenedAtC(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is MonoScript monoScript
                && monoScript.name.Contains(nameof(TypeFieldTest))
                && line == GetLineNumber<C>())
            {
                thisScriptOpenedAtC = true;
                return true;
            }
            return false;
        }

        private static int GetLineNumber<T>()
        {
            var type = typeof(T);
            var sourceInfo = (ProvideSourceInfoAttribute)Attribute.GetCustomAttribute(type, typeof(ProvideSourceInfoAttribute));
            return sourceInfo == null ? -1 : sourceInfo.LineNumber;
        }
    }
}
