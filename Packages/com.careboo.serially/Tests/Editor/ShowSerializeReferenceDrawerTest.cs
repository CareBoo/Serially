using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UEditor = UnityEditor.Editor;

namespace CareBoo.Serially.Editor.Tests
{
    [Serializable]
    public class A
    {
        public int Value;
    }

    [Serializable]
    public class B : A { }

    public class ShowSerializeReferenceDrawerTest : ScriptableObject
    {
        [ShowSerializeReference]
        [SerializeReference]
        public A Field;

        [UnityTest]
        public IEnumerator OnGUIShouldDrawWithoutErrors()
        {
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            var editor = UEditor.CreateEditor(obj);
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => editor.DrawDefaultInspector());
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            testWindow.Close();
        }

        [UnityTest]
        public IEnumerator RightClickInGUIShouldDrawWithoutErrors()
        {
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            var so = new SerializedObject(obj);
            var sp = so.FindProperty(nameof(Field));
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            var position = new Rect(0, 0, 50, 50f);
            var currentEvent = new GuiEvent(EventType.MouseDown, position.center, 1, ShowSerializeReferenceDrawer.RightClickButton);
            testWindow.onGui = new EditorEvent(() => ShowSerializeReferenceDrawer.OnGUI(position, sp, GUIContent.none, currentEvent));
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            testWindow.Close();
        }
    }
}
