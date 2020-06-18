using System;
using System.Linq;

namespace CareBoo.Serially.Editor
{
    public static partial class TypeExtensions
    {
        public static T GetCustomAttribute<T>(this Type type)
            where T : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttributes(typeof(T), true).Select(attr => (T)attr).FirstOrDefault();
        }
    }
}
