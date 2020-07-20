using System;
using UnityEngine;

namespace CareBoo.Serially.Editor.Tests
{
    public class SerializedPropertyExtensionsFixture : ScriptableObject
    {
        [Serializable]
        public class TestClass
        {
            public int IntField;
        }

        [SerializeReference]
        public object SerializeReferenceField;
    }
}
