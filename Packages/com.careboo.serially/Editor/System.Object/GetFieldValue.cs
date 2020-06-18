namespace CareBoo.Serially.Editor
{
    public static partial class SystemObjectExtensions
    {
        public static object GetFieldValue(this object target, string fieldName)
        {
            var field = target.GetFieldInfo(fieldName);
            return field.GetValue(target);
        }
    }
}
