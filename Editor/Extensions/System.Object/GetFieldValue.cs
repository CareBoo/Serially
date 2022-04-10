using System;

namespace CareBoo.Serially.Editor
{
    public static partial class SystemObjectExtensions
    {
        public static object GetFieldValue(this object target, string fieldName)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException("field cannot be null or whitespace", nameof(fieldName));

            var field = target.GetFieldInfo(fieldName);
            if (field != null)
                return field.GetValue(target);

            var property = target.GetPropertyInfo(fieldName);
            if (property != null)
                return property.GetValue(target);

            return null;
        }
    }
}
