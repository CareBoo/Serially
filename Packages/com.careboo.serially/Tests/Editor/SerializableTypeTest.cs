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
    public class SerializableTypeTest : ScriptableObject
    {
        [Guid("e7974cb7-b295-4a9d-b606-824c1fc4c8ea")]
        public class TypeWithGuidAttribute { }

        public class TypeWithoutGuidAttribute { }

        public static object[] TypeCases = new object[]
        {
            new[] { typeof(TypeWithGuidAttribute ) },
            new[] { typeof (TypeWithoutGuidAttribute) }
        };

        public SerializableType Instance;

        public string AssetPath => $"Assets/{nameof(SerializableTypeTest)}.asset";

        [TearDown]
        public void DeleteAsset()
        {
            AssetDatabase.DeleteAsset(AssetPath);
        }

        [Test]
        [Timeout(5000)]
        [TestCaseSource(nameof(TypeCases))]
        public void TryGetTypeShouldReturnSameTypeSerialized(Type expected)
        {
            var typeString = ToSerializedType(expected);
            var isSuccess = TryGetType(typeString, out var actual);
            Assert.IsTrue(isSuccess);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Timeout(5000)]
        public void ToSerializedTypeShouldNotFailWhenGivenNullType()
        {
            var result = ToSerializedType(null);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [Test]
        [Timeout(5000)]
        [TestCaseSource(nameof(TypeCases))]
        public void SetTypeShouldPersistAfterCallingSave(Type expected)
        {
            var asset = CreateInstance<SerializableTypeTest>();
            asset.Instance = new SerializableType(expected);
            AssetDatabase.CreateAsset(asset, AssetPath);
            asset = AssetDatabase.LoadAssetAtPath<SerializableTypeTest>(AssetPath);
            var actual = asset.Instance.Type;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Timeout(5000)]
        public void SetUnknownTypeIdShouldCauseError()
        {
            var asset = CreateInstance<SerializableTypeTest>();
            asset.Instance = new SerializableType();
            AssetDatabase.CreateAsset(asset, AssetPath);
            var serializedObject = new SerializedObject(asset);
            var typeIdProperty = serializedObject.FindProperty("Instance.typeId");
            typeIdProperty.stringValue = "6366a62a-e654-4c53-9da5-d73fb875cd17";
            serializedObject.ApplyModifiedProperties();
            asset = AssetDatabase.LoadAssetAtPath<SerializableTypeTest>(AssetPath);
            var error = asset.Instance.TypeNotFoundError;
            LogAssert.Expect(LogType.Error, error);
        }

        [Test]
        [Timeout(5000)]
        [TestCaseSource(nameof(TypeCases))]
        public void SettingTypeWhenBuildingShouldSetAssemblyQualifiedName(Type expected)
        {
            var asset = CreateInstance<SerializableTypeTest>();
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
        [Timeout(5000)]
        public void IsBuildingShouldBeTrueOnlyDuringBuild()
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
        [Timeout(5000)]
        public void CallbackOrderShouldBe0()
        {
            var actual = new SerializableType();
            Assert.AreEqual(0, actual.callbackOrder);
        }
    }
}
