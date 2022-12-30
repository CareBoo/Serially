using System;
using UnityEditor;
using UnityEngine;
using SerializedPropertyType = UnityEditor.SerializedPropertyType;

namespace CareBoo.Serially.Editor
{
    public static class SerializeReferenceContextMenu
    {
        public struct Context
        {
            public SerializedProperty Property { get; }
            public Type FieldType { get; }

            public Context(SerializedProperty property)
            {
                Property = property;
                FieldType = property.GetManagedReferenceFieldType();
            }

            public void AddToMenu(GenericMenu menu)
            {
                menu.AddItem(new GUIContent(CopyContextName), false, Copy);
                var canPasteCopiedValue = copiedValue != null
                                          && FieldType.IsInstanceOfType(copiedValue);
                if (canPasteCopiedValue)
                    menu.AddItem(new GUIContent(PasteContextName), false, Paste);
                else
                    menu.AddDisabledItem(new GUIContent(PasteContextName));
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

        public const string CopyContextName = "Copy Managed Reference";

        public const string PasteContextName = "Paste Managed Reference";
        
        public static object copiedValue;
        
        [InitializeOnLoadMethod]
        public static void RegisterContextMenuCallback()
        {
            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
        }

        private static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                return;
            }

            var context = new Context(property);
            context.AddToMenu(menu);
        }
    }
}
