using System;
using System.Collections;
using NUnit.Framework;
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
            testWindow.onGui = new EditorEvent(() => ShowSerializeReferenceDrawer.OnGUI(position, sp, GUIContent.none, currentEvent, GetType().GetField(nameof(Field))));
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            testWindow.Close();
        }

        [Test]
        public void ContextCopyShouldCopyPropertyValue()
        {
            var expected = new A() { Value = 32 };
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            obj.Field = expected;
            var so = new SerializedObject(obj);
            var property = so.FindProperty(nameof(Field));
            var context = new ShowSerializeReferenceDrawer.Context(property);
            context.Copy();
            var actual = ShowSerializeReferenceDrawer.copiedValue;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContextPasteShouldPasteValueOntoProperty()
        {
            var expected = new A() { Value = 32 };
            ShowSerializeReferenceDrawer.copiedValue = expected;
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            var so = new SerializedObject(obj);
            var property = so.FindProperty(nameof(Field));
            var context = new ShowSerializeReferenceDrawer.Context(property);
            context.Paste();
            var actual = (A)property.GetValue();
            Assert.AreEqual(expected.Value, actual.Value);
        }

        private ArrayList GetMenuItems(GenericMenu menu)
        {
            return (ArrayList)menu.GetFieldValue("menuItems");
        }

        private string GetMenuItemName(ArrayList menuItems, int index)
        {
            var content = (GUIContent)menuItems[index].GetFieldValue("content");
            return content.text;
        }

        private bool GetMenuItemIsDisabled(ArrayList menuItems, int index)
        {
            return menuItems[index].GetFieldValue("func") == null
                && menuItems[index].GetFieldValue("func2") == null;
        }

        public static object[] ContextCreateMenuCases = new object[]
        {
            new object[] { null, true },
            new object[] { new A(), false },
            new object[] { 32, true }
        };

        [Test]
        [TestCaseSource(nameof(ContextCreateMenuCases))]
        public void ContextCreateMenuShouldReturnMenuWithCopyAndPaste(object copiedValue, bool pasteIsDisabled)
        {
            ShowSerializeReferenceDrawer.copiedValue = copiedValue;
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            var so = new SerializedObject(obj);
            var property = so.FindProperty(nameof(Field));
            var context = new ShowSerializeReferenceDrawer.Context(property);
            var menu = context.CreateMenu();
            var menuItems = GetMenuItems(menu);
            for (var i = 0; i < menuItems.Count; i++)
            {
                var isDisabled = GetMenuItemIsDisabled(menuItems, i);
                if (GetMenuItemName(menuItems, i) == "Copy")
                    Assert.IsFalse(isDisabled);
                else
                    Assert.AreEqual(pasteIsDisabled, isDisabled);
            }
        }
    }
}
