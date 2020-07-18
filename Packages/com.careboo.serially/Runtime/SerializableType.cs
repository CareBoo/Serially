using System;
using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using System.Linq;
#endif // UNITY_EDITOR

namespace CareBoo.Serially
{
    [Serializable]
    public partial class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField]
        protected string typeId = string.Empty;

        public Type Type { get; set; }

#if UNITY_EDITOR
        public static bool isBuilding = false;
#endif // UNITY_EDITOR

        public SerializableType() { }
        public SerializableType(Type type)
        {
            Type = type;
        }

        public static bool TryGetType(string typeString, out Type type)
        {
#if UNITY_EDITOR
            if (Guid.TryParse(typeString, out var guid))
            {
                type = TypeCache.GetTypesWithAttribute(typeof(GuidAttribute))
                    .FirstOrDefault(t => t.GUID == guid);
            }
            else
            {
                type = Type.GetType(typeString);
            }
#else
            type = Type.GetType(typeString);
#endif // UNITY_EDITOR
            return type != null || string.IsNullOrEmpty(typeString);
        }

        public static string ToSerializedType(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }
#if UNITY_EDITOR
            if (Attribute.IsDefined(type, typeof(GuidAttribute)) && !isBuilding)
            {
                return type.GUID.ToString();
            }
            else
            {
                return type.AssemblyQualifiedName;
            }
#else
            return type.AssemblyQualifiedName;
#endif // UNITY_EDITOR
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!TryGetType(typeId, out var type))
            {
                Debug.LogError($"Could not find type for typeId[{typeId}] when trying to deserialize this SerializableType.");
            }
            Type = type;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            var value = ToSerializedType(Type);
            if (typeId != value && !string.IsNullOrEmpty(value))
            {
                typeId = value;
            }
        }
    }

#if UNITY_EDITOR
    public partial class SerializableType : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report)
        {
            isBuilding = false;
        }

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            isBuilding = true;
        }
    }
#endif // UNITY_EDITOR
}
