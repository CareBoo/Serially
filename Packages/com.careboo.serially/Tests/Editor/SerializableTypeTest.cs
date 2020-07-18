using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

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
            var typeString = SerializableType.ToSerializedType(expected);
            var isSuccess = SerializableType.TryGetType(typeString, out var actual);
            Assert.IsTrue(isSuccess);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSerializedTypeDoesNotFailWhenGivenNullType()
        {
            var result = SerializableType.ToSerializedType(null);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [Test]
        [TestCaseSource(nameof(TypeCases))]
        public void SetTypeWithGuidAttributeShouldPersistAfterCallingSave(Type expected)
        {
            var asset = ScriptableObject.CreateInstance<SerializableTypeHolder>();
            asset.Instance = new SerializableType(expected);
            AssetDatabase.CreateAsset(asset, AssetPath);
            asset = AssetDatabase.LoadAssetAtPath<SerializableTypeHolder>(AssetPath);
            var actual = asset.Instance.Type;
            Assert.AreEqual(expected, actual);
        }
    }
}
