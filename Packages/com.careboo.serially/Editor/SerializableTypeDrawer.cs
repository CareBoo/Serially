using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using static CareBoo.Serially.SerializableType;
using static CareBoo.Serially.Editor.SerializableTypeMeta;
using System.Runtime.InteropServices;
using UnityEditor.Compilation;

namespace CareBoo.Serially.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    [CustomPropertyDrawer(typeof(DerivedFromAttribute))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeIdProperty = property.FindPropertyRelative(TypeIdProperty);
            var currentType = ToType(typeIdProperty.stringValue);
            if (currentType != null && !Attribute.IsDefined(currentType, typeof(GuidAttribute)))
            {
                label.image = EditorGUIUtility.IconContent("console.warnicon").image;
                label.tooltip = "The current type doesn't have a GuidAttribute defined. Renaming this type will cause it to lose its reference!";
            }
            else if (currentType == null && !string.IsNullOrEmpty(typeIdProperty.stringValue))
            {
                label.image = EditorGUIUtility.IconContent("console.erroricon").image;
                label.tooltip = $"Type reference could not be found for the typeId, \"{typeIdProperty.stringValue}\". This can happen when renaming a type without a GuidAttribute defined.";
            }

            position = EditorGUI.PrefixLabel(position, label);
            var baseType = GetBaseType(property);
            TypeField(position,
                GetTypeValue(typeIdProperty),
                GetDerivedTypes(baseType),
                SetTypeValue(typeIdProperty));
        }

        private Type GetBaseType(SerializedProperty property)
        {
            switch (attribute)
            {
                case DerivedFromAttribute d when d.Type != null:
                    return d.Type;
                case DerivedFromAttribute d when d.TypeDelegate != null:
                    return d.TypeDelegate();
                case DerivedFromAttribute d when d.TypeDelegateName != null:
                    d.TypeDelegate = CreateTypeDelegate(d.TypeDelegateName, property);
                    return d.TypeDelegate();
                default:
                    return null;
            }
        }

        private Func<Type> CreateTypeDelegate(string name, SerializedProperty property)
        {
            var parentObject = property.GetValue(path => path.SkipLast(1));

            return (Func<Type>)Delegate.CreateDelegate(
                typeof(Func<Type>),
                parentObject,
                name
            );
        }

        private Type[] GetDerivedTypes(Type baseType)
        {
            var derivedTypes = baseType != null
                ? TypeCache.GetTypesDerivedFrom(baseType)
                : CompilationPipeline.GetAssemblies()
                    .Select(assembly => assembly.name)
                    .Select(System.Reflection.Assembly.Load)
                    .SelectMany(a => a.GetExportedTypes());
            return derivedTypes.Where(type => !type.IsGenericType).ToArray();
        }

        private Action<Type> SetTypeValue(SerializedProperty property)
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

        private Type GetTypeValue(SerializedProperty property)
        {
            return ToType(property.stringValue);
        }
    }

    public class SerializableTypeMeta : SerializableType
    {
        public static string TypeIdProperty = nameof(typeId);
    }
}
