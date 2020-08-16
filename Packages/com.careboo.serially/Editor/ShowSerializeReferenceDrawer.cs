﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(ShowSerializeReferenceAttribute))]
    public class ShowSerializeReferenceDrawer : PropertyDrawer
    {
        public const int RightClickButton = 1;
        public static object copiedValue;

        public struct Context
        {
            public SerializedProperty Property { get; }
            public Type FieldType { get; }

            public Context(SerializedProperty property)
            {
                Property = property;
                FieldType = property.GetManagedReferenceFieldType();
            }

            public GenericMenu CreateMenu()
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent(nameof(Copy)), false, Copy);
                if (copiedValue != null && FieldType.IsAssignableFrom(copiedValue.GetType()))
                    menu.AddItem(new GUIContent(nameof(Paste)), false, Paste);
                else
                    menu.AddDisabledItem(new GUIContent(nameof(Paste)));
                return menu;
            }

            public void Copy()
            {
                copiedValue = Property.GetValue();
            }

            public void Paste()
            {
                Property.managedReferenceValue = copiedValue;
                Property.serializedObject.ApplyModifiedProperties();
            }
        }

        private static Rect LabelIndent(Rect position)
        {
            position.xMin += labelWidth + 2f;
            position.height = singleLineHeight;
            return position;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var currentEvent = GuiEvent.FromCurrentUnityEvent;
            OnGUI(position, property, label, currentEvent);
        }

        public static void OnGUI(Rect position, SerializedProperty property, GUIContent label, GuiEvent currentEvent)
        {
            if (currentEvent.Type == EventType.MouseDown
                && position.Contains(currentEvent.MousePosition)
                && currentEvent.Button == RightClickButton)
            {
                var contextMenu = new Context(property).CreateMenu();
                contextMenu.ShowAsContext();
                currentEvent.Type = EventType.Used;
            }

            new TypeField(
                position: LabelIndent(position),
                selectedType: property.GetManagedReferenceValueType(),
                selectableTypes: new Lazy<IEnumerable<Type>>(property.GetSelectableManagedReferenceValueTypes),
                onSelectType: property.SetManagedReferenceValueToNew,
                currentEvent
                ).DrawGui();

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}