using System.Reflection;

namespace CareBoo.Serially.Editor
{
    public static partial class SystemObjectExtensions
    {
        private const BindingFlags FieldFlags = BindingFlags.NonPublic
            | BindingFlags.Public
            | BindingFlags.Instance;

        public static FieldInfo GetFieldInfo(this object target, string propertyStr)
        {
            return target.GetType().GetField(propertyStr, FieldFlags);
        }
    }
}
