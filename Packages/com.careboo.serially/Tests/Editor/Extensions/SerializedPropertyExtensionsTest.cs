using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CareBoo.Serially.Editor.Tests
{
    public class SerializedPropertyExtensionsTest : ScriptableObject
    {
        [Serializable]
        public class TestClass
        {
            public int IntField;
            public int[] ArrayField;
        }

        [Serializable] public class SerializableChildClass : TestClass { }

        public class NonSerializableChildClass : TestClass { }

        [SerializeReference]
        public TestClass SerializeReferenceField;

        public string AssetPath => $"Assets/{nameof(SerializedPropertyExtensionsFixture)}.asset";

        [TearDown]
        public void DeleteFixture()
        {
            AssetDatabase.DeleteAsset(AssetPath);
        }

        public (SerializedProperty, Type) GetNewFixtureProperty(TestClass setFieldValue = null)
        {
            var fixture = CreateInstance<SerializedPropertyExtensionsTest>();
            fixture.SerializeReferenceField = setFieldValue;
            var serializedObject = new SerializedObject(fixture);
            var property = serializedObject.FindProperty(nameof(SerializeReferenceField));
            var propertyType = typeof(TestClass);
            return (property, propertyType);
        }

        [Test]
        public void GetManagedReferenceFieldTypeReturnsFieldType()
        {
            var (serializedProperty, expected) = GetNewFixtureProperty();
            var actual = serializedProperty.GetManagedReferenceFieldType();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetManagedReferenceValueTypeReturnsValueType()
        {
            var (serializedProperty, _) = GetNewFixtureProperty(new SerializableChildClass());
            var expected = typeof(SerializableChildClass);
            var actual = serializedProperty.GetManagedReferenceValueType();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSelectableManagedReferenceValueTypesReturnsSerializableDerivedClasses()
        {
            var expected = new HashSet<Type>(new[] { typeof(TestClass), typeof(SerializableChildClass) });
            var (serializedProperty, _) = GetNewFixtureProperty();
            var actual = new HashSet<Type>(serializedProperty.GetSelectableManagedReferenceValueTypes());
            Assert.AreEqual(expected, actual);
        }
    }
}
