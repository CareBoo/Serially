using System;

namespace CareBoo.Serially.Editor
{
    public static partial class TypeExtensions
    {
        public static bool IsSerializeReferenceable(this Type type)
        {
            return !type.IsGenericType
                && !type.IsAbstract
                && !type.IsInterface
                && Attribute.IsDefined(type, typeof(SerializableAttribute));
        }
    }
}
