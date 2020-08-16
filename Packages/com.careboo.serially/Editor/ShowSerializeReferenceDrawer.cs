﻿using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(ShowSerializeReferenceAttribute))]
    public class ShowSerializeReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = property.GetManagedReferenceValueType();
            var types = property.GetSelectableManagedReferenceValueTypes();
            var cachedPosition = position;
            position.xMin += labelWidth + 2f;
            position.height = singleLineHeight;
            new TypeField(position, type, types, property.SetManagedReferenceValueToNew).DrawGui();

            EditorGUI.PropertyField(cachedPosition, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
