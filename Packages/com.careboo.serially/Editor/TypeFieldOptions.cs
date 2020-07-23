using System;
using UnityEngine;

namespace CareBoo.Serially.Editor
{
    public struct TypeFieldOptions
    {
        public Rect Position { get; }
        public Type SelectedType { get; }
        public Type[] SelectableTypes { get; }
        public Action<Type> OnSelect { get; }

        public TypeFieldOptions(
            Rect position,
            Type selectedType,
            Type[] selectableTypes,
            Action<Type> onSelect
        )
        {
            Position = position;
            SelectedType = selectedType;
            SelectableTypes = selectableTypes;
            OnSelect = onSelect;
        }
    }
}
