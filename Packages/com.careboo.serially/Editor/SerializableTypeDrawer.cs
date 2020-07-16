using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using static CareBoo.Serially.SerializableType;
using static CareBoo.Serially.Editor.SerializableTypeMeta;
using System.Runtime.InteropServices;
using UnityEditor.Compilation;
using System.Collections.Generic;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    [CustomPropertyDrawer(typeof(TypeFilterAttribute))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeIdProperty = property.FindPropertyRelative(TypeIdProperty);
            var type = Validate(typeIdProperty, label);

            position = EditorGUI.PrefixLabel(position, label);
            TypeField(
                position,
                type,
                GetFilteredTypes(property).ToArray(),
                SetTypeValue(typeIdProperty)
                );
        }

        public Type Validate(SerializedProperty typeIdProperty, GUIContent label)
        {
            var typeReferenceExists = TryGetType(typeIdProperty.stringValue, out Type type);
            var typeId = typeIdProperty.stringValue;
            if (!typeReferenceExists)
            {
                label.image = EditorGUIUtility.IconContent("console.erroricon").image;
                label.tooltip = $"Type reference could not be found for the typeId, \"{typeId}\". This can happen when renaming a type without a GuidAttribute defined.";
            }
            else if (IsMissingGuidAttribute(type))
            {
                label.image = EditorGUIUtility.IconContent("console.warnicon").image;
                label.tooltip = "The current type doesn't have a GuidAttribute defined. Renaming this type will cause it to lose its reference!";
            }
            return type;
        }

        public bool IsMissingGuidAttribute(Type type)
        {
            return type != null && !Attribute.IsDefined(type, typeof(GuidAttribute));
        }

        public IEnumerable<Type> GetFilteredTypes(SerializedProperty property)
        {
            var (baseType, filter) = GetTypeFilter(property);
            return GetDerivedTypes(baseType).Where(filter);
        }

        public (Type baseType, Func<Type, bool> filter) GetTypeFilter(SerializedProperty property)
        {
            if (attribute is TypeFilterAttribute t)
            {
                var parentObject = property.GetValue(p => p.SkipLast(1));
                return (t.DerivedFrom, t.GetFilter(parentObject));
            }
            return (null, _ => true);
        }

        public IEnumerable<Type> GetDerivedTypes(Type baseType)
        {
            var derivedTypes = baseType != null
                ? TypeCache.GetTypesDerivedFrom(baseType)
                : CompilationPipeline.GetAssemblies()
                    .Select(assembly => assembly.name)
                    .Select(System.Reflection.Assembly.Load)
                    .SelectMany(a => a.GetExportedTypes());
            return derivedTypes.Where(type => !type.IsGenericType);
        }

        public Action<Type> SetTypeValue(SerializedProperty property)
        {
            return type =>
            {
                var value = ToSerializedType(type);
                if (property.stringValue != value && !string.IsNullOrEmpty(value))
                {
                    property.stringValue = value;
                    property.serializedObject.ApplyModifiedProperties();
                }
            };
        }
    }

    public class SerializableTypeMeta : SerializableType
    {
        public static string TypeIdProperty = nameof(typeId);
    }
}
