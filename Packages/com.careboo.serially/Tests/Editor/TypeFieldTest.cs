using UnityEngine;
using NUnit.Framework;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using UnityEngine.TestTools;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.Events;

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
        private static Type[] Types => new[] { typeof(A), typeof(B), typeof(C) };

        [TearDown]
        public void CloseTestWindow()
        {
            if (EditorWindow.HasOpenInstances<TestEditorWindow>())
                EditorWindow.GetWindow<TestEditorWindow>().Close();
        }

        [Test]
        public void ClickingTheTypeFieldOfTypeWithProvideSourceInfoShouldPingTheMonoScript()
        {
            var guiEvent = new GUIEvent(EventType.MouseDown, Vector2.zero, 1);
            var typeFieldOptions = new TypeFieldOptions(Rect.zero, typeof(C), null, null);
            var monoScript = HandleTypeLabelClicked(typeFieldOptions, guiEvent);
            Assert.IsNotNull(monoScript);
        }

        [Test]
        public void ClickingTheTypeFieldOfTypeWithoutProvideSourceInfoShouldLogWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(nameof(ProvideSourceInfoAttribute)));
            var guiEvent = new GUIEvent(EventType.MouseDown, Vector2.zero, 1);
            var typeFieldOptions = new TypeFieldOptions(Rect.zero, typeof(B), null, null);
            var monoScript = HandleTypeLabelClicked(typeFieldOptions, guiEvent);
            Assert.IsNull(monoScript);
        }

        [UnityTest]
        public IEnumerator ClickingTypePickerShouldOpenTypePickerWindow()
        {
            var typeFieldOptions = new TypeFieldOptions(
                TypeFieldRect,
                typeof(A),
                Types,
                null
                );
            var pickerArea = GetTypePickerButtonPosition(typeFieldOptions.Position);
            var guiEvent = new GUIEvent(
                EventType.MouseDown,
                pickerArea.center,
                1
                );
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => TypeField(typeFieldOptions, guiEvent));
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            Assert.IsTrue(EditorWindow.HasOpenInstances<TypePickerWindow>());
            EditorWindow.GetWindow<TypePickerWindow>().Close();
        }

        [UnityTest]
        public IEnumerator ClickingTypeLabelTwiceForTypeDefinedInThisAssetShouldOpenThisAsset()
        {
            var typeFieldOptions = new TypeFieldOptions(
                TypeFieldRect,
                typeof(C),
                Types,
                null
                );
            var guiEvent = new GUIEvent(
                EventType.MouseDown,
                TypeFieldRect.center,
                2
                );
            thisScriptOpenedAtC = false;
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => TypeField(typeFieldOptions, guiEvent));
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            Assert.IsTrue(thisScriptOpenedAtC);
        }

        [UnityTest]
        public IEnumerator TypeFieldShouldShowWithoutErrors()
        {
            var type = typeof(A);
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => TypeField(TypeFieldRect, type, Types, null));
            yield return new WaitUntil(testWindow.OnGUIInitialized);
        }

        [OnOpenAsset(1)]
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
