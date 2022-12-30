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
            yield return new WaitUntil(testWindow.OnGUIInitialized).OrTimeout(2000);
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
            var context = new SerializeReferenceContextMenu.Context(property);
            context.Copy();
            var actual = SerializeReferenceContextMenu.copiedValue;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Timeout(5000)]
        public void ContextPasteShouldPasteValueOntoProperty()
        {
            var expected = new A() { Value = 32 };
            SerializeReferenceContextMenu.copiedValue = expected;
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            var so = new SerializedObject(obj);
            var property = so.FindProperty(nameof(Field));
            var context = new SerializeReferenceContextMenu.Context(property);
            context.Paste();
            var actual = (A)property.GetValue();
            Assert.AreEqual(expected.Value, actual.Value);
        }

        private IList GetMenuItems(GenericMenu menu)
        {
            return (IList)menu.GetFieldValue("menuItems");
        }

        private string GetMenuItemName(IList menuItems, int index)
        {
            var content = (GUIContent)menuItems[index].GetFieldValue("content");
            return content.text;
        }

        private bool GetMenuItemIsDisabled(IList menuItems, int index)
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
            SerializeReferenceContextMenu.copiedValue = copiedValue;
            var obj = CreateInstance<ShowSerializeReferenceDrawerTest>();
            var so = new SerializedObject(obj);
            var property = so.FindProperty(nameof(Field));
            var context = new SerializeReferenceContextMenu.Context(property);
            var menu = new GenericMenu();
            context.AddToMenu(menu);
            var menuItems = GetMenuItems(menu);
            for (var i = 0; i < menuItems.Count; i++)
            {
                var isDisabled = GetMenuItemIsDisabled(menuItems, i);
                var menuItemName = GetMenuItemName(menuItems, i);
                switch (menuItemName)
                {
                    case SerializeReferenceContextMenu.CopyContextName:
                        Assert.IsFalse(isDisabled);
                        break;
                    case SerializeReferenceContextMenu.PasteContextName:
                        Assert.AreEqual(pasteIsDisabled, isDisabled);
                        break;
                    default:
                        throw new NotSupportedException($"Found an unsupported menu item: {menuItemName}");
                }
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
