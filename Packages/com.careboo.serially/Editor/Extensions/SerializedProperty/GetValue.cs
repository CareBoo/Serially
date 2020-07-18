using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CareBoo.Serially.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static object GetValue(this SerializedProperty property,
            Func<IEnumerable<string>, IEnumerable<string>> pathModifier = null)
        {
            IEnumerable<string> path = property.propertyPath.Replace(".Array.data[", "[").Split('.');
            if (pathModifier != null) path = pathModifier(path);

            var target = (object)property.serializedObject.targetObject;
            return target.GetValueRecur(path);
        }

        private static object GetValueRecur(this object target, IEnumerable<string> propertyPath)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            var propertyStr = propertyPath.FirstOrDefault();
            if (propertyStr == null) return target;

            target = propertyStr.TryGetArrayIndex(out propertyStr, out int index)
                ? (target.GetFieldValue(propertyStr) as IEnumerable).ElementAtOrDefault(index)
                : target.GetFieldValue(propertyStr);

            return target.GetValueRecur(propertyPath.Skip(1));
        }
    }
}
