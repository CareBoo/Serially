using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(ShowSerializeReferenceAttribute))]
    public class ShowSerializeReferenceDrawer : PropertyDrawer
    {
        private static (Rect label, Rect property) LabelPositions(Rect position)
        {
            var label = new Rect(position.x, position.y, labelWidth, singleLineHeight);
            var property = new Rect(position.x + labelWidth + 2f, position.y, position.width - labelWidth - 2f,
                singleLineHeight);

            return (label, property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var currentEvent = GuiEvent.FromCurrentUnityEvent;
            currentEvent = OnGUI(position, property, label, currentEvent, fieldInfo);
            if (currentEvent.Type == EventType.Used)
            {
                Event.current.Use();
            }
        }

        public static GuiEvent OnGUI(Rect position, SerializedProperty property, GUIContent label, GuiEvent currentEvent, FieldInfo fieldInfo)
        {
            var (labelPosition, propertyPosition) = LabelPositions(position);

            var selectableTypes = new Lazy<IEnumerable<Type>>(() => GetSelectableTypes(property, fieldInfo));
            new TypeField(
                position: propertyPosition,
                selectedType: property.GetManagedReferenceValueType(),
                selectableTypes: selectableTypes,
                onSelectType: property.SetManagedReferenceValueToNew,
                currentEvent
                ).DrawGui();

            EditorGUI.PropertyField(position, property, label, true);
            return currentEvent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public static IEnumerable<Type> GetSelectableTypes(SerializedProperty property, FieldInfo fieldInfo)
        {
            IEnumerable<Type> selectableTypes = property.GetSelectableManagedReferenceValueTypes();
            var typeFilterAttribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
            if (typeFilterAttribute != null)
            {
                var parentObject = property.GetValue(p => p.SkipLast(1));
                var filter = typeFilterAttribute?.GetFilter(parentObject);
                selectableTypes = filter(selectableTypes);
            }
            return selectableTypes ?? Enumerable.Empty<Type>();
        }
    }
}
