using System;
using System.Diagnostics;
using UnityEngine;

namespace CareBoo.Serially
{
    [Conditional("UNITY_EDITOR")]
    public class DerivedFromAttribute : PropertyAttribute
    {
        public Type Type { get; }

        public Func<Type> TypeDelegate { get; set; }

        public string TypeDelegateName { get; }

        public DerivedFromAttribute(Type type)
        {
            Type = type;
        }

        public DerivedFromAttribute(string typeDelegateName)
        {
            TypeDelegateName = typeDelegateName;
        }
    }
}
