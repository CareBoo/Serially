using System;
using UnityEditor;
using static CareBoo.Serially.Editor.TypeExtensions;

namespace CareBoo.Serially.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static Type GetManagedReferenceFieldType(this SerializedProperty property)
        {
            return GetTypeFromManagedReferenceFullTypename(property.managedReferenceFieldTypename, out var type)
                ? type
                : null;
        }
    }
}
