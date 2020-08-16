using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static CareBoo.Serially.SerializableType;
using static CareBoo.Serially.Editor.SerializableTypeMeta;
using System.Runtime.InteropServices;
using UnityEditor.Compilation;
using System.Collections.Generic;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeIdProperty = property.FindPropertyRelative(TypeIdProperty);
            var type = Validate(typeIdProperty.stringValue, label);

            position = EditorGUI.PrefixLabel(position, label);
            var typeFilterAttribute = (TypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
            var selectableTypes = new Lazy<IEnumerable<Type>>(() => GetFilteredTypes(property, typeFilterAttribute).ToArray());
            new TypeField(
                position,
                type,
                selectableTypes,
                SetTypeValue(typeIdProperty)
                )
                .DrawGui();
        }

        public static Type Validate(string typeId, GUIContent label)
        {
            var typeReferenceExists = TryGetType(typeId, out Type type);
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

        public static bool IsMissingGuidAttribute(Type type)
        {
            return type != null && !Attribute.IsDefined(type, typeof(GuidAttribute));
        }

        public static IEnumerable<Type> GetFilteredTypes(SerializedProperty property, PropertyAttribute propertyAttribute)
        {
            var (baseType, filter) = GetTypeFilter(property, propertyAttribute);
            return filter.Invoke(GetDerivedTypes(baseType));
        }

        public static (Type baseType, Func<IEnumerable<Type>, IEnumerable<Type>> filter) GetTypeFilter(SerializedProperty property, PropertyAttribute propertyAttribute)
        {
            if (propertyAttribute is TypeFilterAttribute t)
            {
                var parentObject = property.GetValue(p => p.SkipLast(1));
                return (t.DerivedFrom, t.GetFilter(parentObject));
            }
            return (null, sequence => sequence);
        }

        public static IEnumerable<Type> GetDerivedTypes(Type baseType)
        {
            var derivedTypes = baseType != null
                ? TypeCache.GetTypesDerivedFrom(baseType)
                : CompilationPipeline.GetAssemblies()
                    .Select(assembly => assembly.name)
                    .Select(System.Reflection.Assembly.Load)
                    .SelectMany(a => a.GetExportedTypes());
            return derivedTypes.Where(type => !type.IsGenericType);
        }

        public static Action<Type> SetTypeValue(SerializedProperty property)
        {
            return type =>
            {
                var value = ToSerializedType(type);
                if (property.stringValue != value)
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
