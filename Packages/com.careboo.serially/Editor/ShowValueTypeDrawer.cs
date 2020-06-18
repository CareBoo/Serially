using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;
using static CareBoo.Serially.Editor.EditorGUIExtensions;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(ShowValueTypeAttribute))]
    public class ShowValueTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = property.GetManagedReferenceValueType();
            var types = property.GetSelectableManagedReferenceValueTypes();
            var cachedPosition = position;
            position.xMin += labelWidth;
            position.height = singleLineHeight;
            TypeField(position, type, types, property.SetManagedReferenceValueToNew);

            EditorGUI.PropertyField(cachedPosition, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
