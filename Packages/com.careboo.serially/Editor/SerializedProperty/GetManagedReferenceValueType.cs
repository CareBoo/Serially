using System;
using UnityEditor;
using static CareBoo.Serially.Editor.TypeExtensions;

namespace CareBoo.Serially.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static Type GetManagedReferenceValueType(this SerializedProperty property)
        {
            return GetTypeFromManagedReferenceFullTypename(property.managedReferenceFullTypename, out var type)
                ? type
                : null;
        }
    }
}
