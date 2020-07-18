using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.TestTools;
using static CareBoo.Serially.SerializableType;

namespace CareBoo.Serially.Editor.Tests
{
    public class SerializableTypeTest
    {
        [Guid("e7974cb7-b295-4a9d-b606-824c1fc4c8ea")]
        public class TypeWithGuidAttribute { }

        public class TypeWithoutGuidAttribute { }

        public static object[] TypeCases = new object[]
        {
            new[] { typeof(TypeWithGuidAttribute ) },
            new[] { typeof (TypeWithoutGuidAttribute) }
        };

        public string AssetPath => $"Assets/{nameof(SerializableTypeHolder)}.asset";

        [TearDown]
        public void DeleteAsset()
        {
            AssetDatabase.DeleteAsset(AssetPath);
        }

        [Test]
        [TestCaseSource(nameof(TypeCases))]
        public void TryGetTypeShouldReturnSameTypeSerialized(Type expected)
        {
            var typeString = ToSerializedType(expected);
            var isSuccess = TryGetType(typeString, out var actual);
            Assert.IsTrue(isSuccess);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSerializedTypeDoesNotFailWhenGivenNullType()
        {
            var result = ToSerializedType(null);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [Test]
        [TestCaseSource(nameof(TypeCases))]
        public void SetTypeShouldPersistAfterCallingSave(Type expected)
        {
            var asset = ScriptableObject.CreateInstance<SerializableTypeHolder>();
            asset.Instance = new SerializableType(expected);
            AssetDatabase.CreateAsset(asset, AssetPath);
            asset = AssetDatabase.LoadAssetAtPath<SerializableTypeHolder>(AssetPath);
            var actual = asset.Instance.Type;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetInvalidTypeIdShouldError()
        {
            var asset = ScriptableObject.CreateInstance<SerializableTypeHolder>();
            asset.Instance = new SerializableType();
            AssetDatabase.CreateAsset(asset, AssetPath);
            var serializedObject = new SerializedObject(asset);
            var typeIdProperty = serializedObject.FindProperty("Instance.typeId");
            typeIdProperty.stringValue = "6366a62a-e654-4c53-9da5-d73fb875cd17";
            serializedObject.ApplyModifiedProperties();
            asset = AssetDatabase.LoadAssetAtPath<SerializableTypeHolder>(AssetPath);
            var error = asset.Instance.TypeNotFoundError;
            LogAssert.Expect(LogType.Error, error);
        }

        [Test]
        [TestCaseSource(nameof(TypeCases))]
        public void SettingTypeWhenBuildingSetsAssemblyQualifiedName(Type expected)
        {
            var asset = ScriptableObject.CreateInstance<SerializableTypeHolder>();
            var serializableType = new SerializableType(expected);
            asset.Instance = new SerializableType(expected);
            (serializableType as IPreprocessBuildWithReport).OnPreprocessBuild(null);
            AssetDatabase.CreateAsset(asset, AssetPath);
            var serializedObject = new SerializedObject(asset);
            var typeIdProperty = serializedObject.FindProperty("Instance.typeId");
            var typeId = typeIdProperty.stringValue;
            Assert.AreEqual(expected.AssemblyQualifiedName, typeId);
            (serializableType as IPostprocessBuildWithReport).OnPostprocessBuild(null);
        }

        [Test]
        public void IsBuildingIsTrueOnlyDuringBuild()
        {
            var serializableType = new SerializableType();
            var actual = IsBuilding;
            Assert.IsFalse(actual);
            (serializableType as IPreprocessBuildWithReport).OnPreprocessBuild(null);
            actual = IsBuilding;
            Assert.IsTrue(actual);
            (serializableType as IPostprocessBuildWithReport).OnPostprocessBuild(null);
            actual = IsBuilding;
            Assert.IsFalse(actual);
        }

        [Test]
        public void CallbackOrderIs0()
        {
            var actual = new SerializableType();
            Assert.AreEqual(0, actual.callbackOrder);
        }
    }
}
