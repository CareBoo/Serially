using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CareBoo.Serially.Editor.Tests.Reflection
{
    public static partial class ReflectionUtil
    {
        public static MemberInfo GetMemberInfo<TSource, TMember>(
            Expression<Func<TSource, TMember>> selector)
        {
            var body = selector.Body as MemberExpression;
            return body.Member;
        }
    }
}
