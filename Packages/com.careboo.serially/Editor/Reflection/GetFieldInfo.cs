using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CareBoo.Serially.Editor.Reflection
{
    public static partial class ReflectionUtil
    {
        public static FieldInfo GetFieldInfo<TSource>(
            Expression<Func<TSource, object>> selector)
        {
            var memberInfo = GetMemberInfo(selector);
            if (memberInfo.MemberType != MemberTypes.Field)
                throw new ArgumentException("Member is not a field!");
            return (FieldInfo)memberInfo;
        }
    }
}
