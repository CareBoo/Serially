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

        public Action<Type, MouseDownEvent> OnClick { get; }

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

        public TypePickerListElement(Type type, Action<Type, MouseDownEvent> onClick)
        {
            Type = type;
            OnClick = onClick;
            VisualElement = VisualTreeAsset.CloneTree().GetFirstOfType<VisualElement>();
            var label = VisualElement.Q<Label>(name: TypeLabelName);
            label.text = GetTypeLabelString(Type);
            VisualElement.RegisterCallback<MouseDownEvent>(MouseDownCallback);
        }

        public void MouseDownCallback(MouseDownEvent evt)
        {
            OnClick?.Invoke(Type, evt);
        }

        public void SetHighlight(bool isHighlighted)
        {

        }

        public static implicit operator VisualElement(TypePickerListElement source)
        {
            return source.VisualElement;
        }
    }
}
