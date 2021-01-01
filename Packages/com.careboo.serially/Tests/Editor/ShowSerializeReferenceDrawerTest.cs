using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeReference]
        [ShowSerializeReference]
        public A Field;

        [SerializeReference]
        [ShowSerializeReference]
        [TypeFilter(nameof(Filter))]
        public A FilteredField;

        public Func<IEnumerable<Type>, IEnumerable<Type>> typeFilter;

        public IEnumerable<Type> Filter(IEnumerable<Type> types)
        {
            return typeFilter.Invoke(types);
        }

        [UnityTest]
        [Timeout(5000)]
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
        [Timeout(5000)]
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
        [Timeout(5000)]
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
        [Timeout(5000)]
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
        [Timeout(5000)]
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

        public static Func<IEnumerable<Type>, IEnumerable<Type>> CreateFilter(Func<IEnumerable<Type>, IEnumerable<Type>> filter) => filter;

        public static IEnumerable<Type> NullFilterDelegate(IEnumerable<Type> types)
        {
            return null;
        }

        public static IEnumerable<Type> BOnlyFilterDelegtae(IEnumerable<Type> types)
        {
            return types.Where(t => t == typeof(B));
        }

        public static object[] TypeFilterCases = new object[]
        {
            new object[] { CreateFilter(BOnlyFilterDelegtae) },
            new object[] { CreateFilter(NullFilterDelegate) }
        };

        [Test]
        [Timeout(5000)]
        [TestCaseSource(nameof(TypeFilterCases))]
        public void GetSelectableTypesShouldReturnTypesFilteredByTypeFilter(
            Func<IEnumerable<Type>, IEnumerable<Type>> filter)
        {
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            obj.typeFilter = filter;
            var so = new SerializedObject(obj);
            var property = so.FindProperty(nameof(FilteredField));
            var fieldInfo = GetType().GetField(nameof(FilteredField));
            var expectedTypes = filter(property.GetSelectableManagedReferenceValueTypes()) ?? new Type[0];
            var actualTypes = ShowSerializeReferenceDrawer.GetSelectableTypes(property, fieldInfo);
            var expected = new HashSet<Type>(expectedTypes);
            var actual = new HashSet<Type>(actualTypes);
            Assert.AreEqual(expected, actual);
        }
    }
}
