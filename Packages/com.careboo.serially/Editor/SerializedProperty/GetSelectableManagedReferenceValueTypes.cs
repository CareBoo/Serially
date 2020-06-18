using System;
using System.Linq;
using UnityEditor;
using static UnityEditor.TypeCache;

namespace CareBoo.Serially.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static Type[] GetSelectableManagedReferenceValueTypes(this SerializedProperty property)
        {
            var baseType = property.GetManagedReferenceFieldType();
            if (baseType == null) throw new ArgumentException(nameof(property));

            return GetTypesDerivedFrom(baseType)
                .Prepend(baseType)
                .Where(TypeExtensions.IsSerializeReferenceable)
                .ToArray();
        }
    }
}
