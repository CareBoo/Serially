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

    public class ShowValueTypeDrawerTest : ScriptableObject
    {
        [ShowValueType]
        [SerializeReference]
        public A Field;

        [UnityTest]
        public IEnumerator OnGUIShouldDrawWithoutErrors()
        {
            var obj = CreateInstance<ShowValueTypeDrawerTest>();
            var editor = UEditor.CreateEditor(obj);
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => editor.DrawDefaultInspector());
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            testWindow.Close();
        }
    }
}
