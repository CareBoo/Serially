using System;
using UnityEngine;
using UnityEngine.UIElements;
using static CareBoo.Serially.Editor.EditorGUIExtensions;

namespace CareBoo.Serially.Editor
{
    public class TypePickerListElement
    {
        public const string TypeLabelName = "type-label";

        public Type Type { get; }

        public Action<TypePickerListElement, MouseDownEvent> OnClicked { get; }

        public VisualElement VisualElement { get; }

        private static VisualTreeAsset visualTreeAsset;

        private static VisualTreeAsset VisualTreeAsset =>
            visualTreeAsset ?? LoadVisualTreeAsset();

        private static VisualTreeAsset LoadVisualTreeAsset()
        {
            var assetPath = nameof(TypePickerListElement).ToSnakeCase();
            visualTreeAsset = Resources.Load<VisualTreeAsset>(assetPath);
            return visualTreeAsset;
        }

        public TypePickerListElement(Type type, Action<TypePickerListElement, MouseDownEvent> onClicked)
        {
            Type = type;
            OnClicked = onClicked;
            VisualElement = VisualTreeAsset.CloneTree().GetFirstOfType<VisualElement>();
            var label = VisualElement.Q<Label>(name: TypeLabelName);
            label.text = GetTypeLabelString(Type);
        }

        private void HandleClicked(MouseDownEvent evt)
        {
            OnClicked?.Invoke(this, evt);
        }

        public static implicit operator VisualElement(TypePickerListElement source)
        {
            return source.VisualElement;
        }
    }
}
