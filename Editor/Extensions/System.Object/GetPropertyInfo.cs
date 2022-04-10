using System.Reflection;

namespace CareBoo.Serially.Editor
{
    public static partial class SystemObjectExtensions
    {
        private const BindingFlags PropertyFlags = BindingFlags.NonPublic
            | BindingFlags.Public
            | BindingFlags.Instance;

        public static PropertyInfo GetPropertyInfo(this object target, string propertyStr)
        {
            return target.GetType().GetProperty(propertyStr, PropertyFlags);
        }
    }
}
