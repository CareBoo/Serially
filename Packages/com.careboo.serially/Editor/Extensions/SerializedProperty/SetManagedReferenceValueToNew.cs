using System;
using UnityEditor;

namespace CareBoo.Serially.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static void SetManagedReferenceValueToNew(this SerializedProperty property, Type type)
        {
            property.managedReferenceValue = type != null
                ? Activator.CreateInstance(type)
                : null;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
