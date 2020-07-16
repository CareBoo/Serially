using System;
using System.Diagnostics;
using UnityEngine;

namespace CareBoo.Serially
{
    [Conditional("UNITY_EDITOR")]
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Type DerivedFrom { get; }

        public Func<Type, bool> FilterDelegate { get; protected set; }

        public string FilterDelegateName { get; }

        public TypeFilterAttribute(Type derivedFrom)
        {
            DerivedFrom = derivedFrom;
            FilterDelegate = _ => true;
        }

        public TypeFilterAttribute(string filterDelegateName)
        {
            FilterDelegateName = filterDelegateName;
        }

        public Func<Type, bool> GetFilter(object parent)
        {
            return FilterDelegate ?? BindFilterDelegate(parent);
        }

        public Func<Type, bool> BindFilterDelegate(object parent)
        {
            FilterDelegate = (Func<Type, bool>)Delegate.CreateDelegate(
                typeof(Func<Type, bool>),
                parent,
                FilterDelegateName
                );
            return FilterDelegate;
        }
    }
}
