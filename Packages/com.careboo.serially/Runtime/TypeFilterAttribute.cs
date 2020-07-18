using System;
using System.Diagnostics;
using UnityEngine;

namespace CareBoo.Serially
{
    [Conditional("UNITY_EDITOR")]
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Type DerivedFrom { get; }

        public Func<Type, bool> DerivedFromFilter => _ => true;

        public Func<Type, bool> Filter { get; protected set; }

        public string FilterName { get; }

        public TypeFilterAttribute(Type derivedFrom)
        {
            DerivedFrom = derivedFrom;
            Filter = DerivedFromFilter;
        }

        public TypeFilterAttribute(string filterName)
        {
            FilterName = filterName;
        }

        public Func<Type, bool> GetFilter(object parent)
        {
            return Filter ?? BindFilterDelegate(parent);
        }

        public Func<Type, bool> BindFilterDelegate(object parent)
        {
            Filter = (Func<Type, bool>)Delegate.CreateDelegate(
                typeof(Func<Type, bool>),
                parent,
                FilterName
                );
            return Filter;
        }
    }
}
