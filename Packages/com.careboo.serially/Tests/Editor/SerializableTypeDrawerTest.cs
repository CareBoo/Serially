using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static CareBoo.Serially.Editor.Tests.Reflection.ReflectionUtil;
using static CareBoo.Serially.Editor.SerializableTypeMeta;
using UnityEngine.TestTools;
using System.Collections;
using UEditor = UnityEditor.Editor;

namespace CareBoo.Serially.Editor.Tests
{
    public class SerializableTypeDrawerTest : ScriptableObject
    {
        public class A { }

        [Guid("d36e9925-bd5f-49fb-9315-603538cbfea0")]
        public class B { }

        public SerializableType NoTypeFilter;

        [TypeFilter(derivedFrom: typeof(A))]
        public SerializableType DerivedFromTypeFilter;

        [TypeFilter(nameof(Filter))]
        public SerializableType DelegateTypeFilter;

        public IEnumerable<Type> Filter(IEnumerable<Type> sequence)
        {
            return sequence.Where(t => t == typeof(B));
        }

        [Test]
        public void ValidateWithInvalidTypeIDShouldSetError()
        {
            var expected = EditorGUIUtility.IconContent("console.erroricon").image;
            var guiContent = new GUIContent();
            SerializableTypeDrawer.Validate("invalid", guiContent);
            var actual = guiContent.image;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateWithMissingGuidAttributeShouldSetWarning()
        {
            var expected = EditorGUIUtility.IconContent("console.warnicon").image;
            var guiContent = new GUIContent();
            SerializableTypeDrawer.Validate(typeof(A).AssemblyQualifiedName, guiContent);
            var actual = guiContent.image;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateWithMissingGuidAttributeShouldReturnType()
        {
            var expected = typeof(A);
            var actual = SerializableTypeDrawer.Validate(expected.AssemblyQualifiedName, new GUIContent());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateWithGuidAttributeShouldReturnType()
        {
            var expected = typeof(B);
            var actual = SerializableTypeDrawer.Validate(expected.GUID.ToString(), new GUIContent());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsMissingGuidAttributeShouldReturnTrueIfNoGuidAttributeDefined()
        {
            Assert.IsTrue(SerializableTypeDrawer.IsMissingGuidAttribute(typeof(A)));
        }

        [Test]
        public void IsMissingGuidAttributeShouldReturnFalseIfGuidAttributeDefined()
        {
            Assert.IsFalse(SerializableTypeDrawer.IsMissingGuidAttribute(typeof(B)));
        }

        [Test]
        public void GetFilteredTypesShouldReturnAllBasicTypesWithNoFilter()
        {
            var expected = new HashSet<Type>(SerializableTypeDrawer.GetDerivedTypes(null));
            var property = GetProperty(nameof(NoTypeFilter));
            var actual = new HashSet<Type>(SerializableTypeDrawer.GetFilteredTypes(property, null));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetFilteredTypesWithDerivedFromAttributeShouldReturnOnlyTypesDerivedFromDeclaredType()
        {
            var expected = new HashSet<Type>(SerializableTypeDrawer.GetDerivedTypes(typeof(A)));
            var field = GetFieldInfo<SerializableTypeDrawerTest>(x => x.DerivedFromTypeFilter);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var property = GetProperty(nameof(DerivedFromTypeFilter));
            var actual = new HashSet<Type>(SerializableTypeDrawer.GetFilteredTypes(property, attribute));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetFilteredTypesWithTypeDelegateShouldReturnTypesMatchingDelegate()
        {
            var expected = new HashSet<Type>(Filter(SerializableTypeDrawer.GetDerivedTypes(null)));
            var field = GetFieldInfo<SerializableTypeDrawerTest>(x => x.DelegateTypeFilter);
            var attribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TypeFilterAttribute));
            var property = GetProperty(nameof(DelegateTypeFilter));
            var actual = new HashSet<Type>(SerializableTypeDrawer.GetFilteredTypes(property, attribute));
            Assert.AreEqual(expected, actual);
        }

        public static object[] SetTypeCases = new object[]
        {
            new object[] { typeof(A) },
            new object[] { typeof(B) },
            new object[] { null }
        };

        [Test]
        [TestCaseSource(nameof(SetTypeCases))]
        public void SetTypeValueShouldReturnSameType(Type expected)
        {
            var property = GetProperty(nameof(NoTypeFilter));
            var typeIdProperty = property.FindPropertyRelative(TypeIdProperty);
            var setTypeValue = SerializableTypeDrawer.SetTypeValue(typeIdProperty);
            setTypeValue(expected);
            var actual = SerializableTypeDrawer.Validate(typeIdProperty.stringValue, new GUIContent());
            Assert.AreEqual(expected, actual);
        }

        [UnityTest]
        public IEnumerator SerializableTypeDrawerShouldShowWithoutErrors()
        {
            var editor = UEditor.CreateEditor(this);
            var testWindow = EditorWindow.GetWindow<TestEditorWindow>();
            testWindow.onGui = new EditorEvent(() => editor.DrawDefaultInspector());
            yield return new WaitUntil(testWindow.OnGUIInitialized);
            testWindow.Close();
        }

        private SerializedProperty GetProperty(string path)
        {
            var serializedObject = new SerializedObject(this);
            return serializedObject.FindProperty(path);
        }
    }
}
