using System;
using UnityEngine;
using UnityEngine.UIElements;
using static CareBoo.Serially.Editor.EditorGUIExtensions;

namespace CareBoo.Serially.Editor
{
    public class TypePickerListElement
    {
        public Type Type { get; }

        public Action<Type> OnSelect { get; }

        public EventCallback<MouseDownEvent> MouseDownCallback { get; }

        private static VisualTreeAsset visualTreeAsset;

        private static VisualTreeAsset VisualTreeAsset =>
            visualTreeAsset ?? LoadVisualTreeAsset();

        private static VisualTreeAsset LoadVisualTreeAsset()
        {
            var assetPath = nameof(TypePickerListElement).ToSnakeCase();
            visualTreeAsset = Resources.Load<VisualTreeAsset>(assetPath);
            return visualTreeAsset;
        }

        public TypePickerListElement(Type type, Action<Type> onSelect)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            OnSelect = onSelect ?? throw new ArgumentNullException(nameof(onSelect));
            MouseDownCallback = _ => OnSelect(type);
        }

        public static implicit operator VisualElement(TypePickerListElement source)
        {
            var result = VisualTreeAsset.CloneTree().GetFirstOfType<VisualElement>();
            var label = result.Q<Label>(name: "type-label");
            label.text = GetTypeLabelString(source.Type);
            result.RegisterCallback(source.MouseDownCallback);
            return result;
        }
    }
}
