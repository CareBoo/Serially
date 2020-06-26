using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;
using static UnityEditor.AssetDatabase;
using static UnityEditor.EditorGUIUtility;

namespace CareBoo.Serially.Editor
{
    public static partial class EditorGUIExtensions
    {
        private static readonly GUIStyle pickButtonStyle = "ObjectFieldButton";

        public static Rect TypeField(
            Rect position,
            Type type,
            Type[] types,
            Action<Type> onSelect)
        {
            var pickerArea = GetPickerArea(position);
            var evt = Event.current;
            if (evt.type == EventType.MouseDown)
            {
                if (pickerArea.Contains(evt.mousePosition))
                    SelectTypeWindow.ShowWindow(type, types, onSelect);
                else if (position.Contains(evt.mousePosition))
                    HandleTypeLabelClicked(evt, type);
            }
            DrawTypeLabel(position, type);
            DrawTypePicker(pickerArea);

            position.y += singleLineHeight + standardVerticalSpacing;
            return position;
        }

        private static void HandleTypeLabelClicked(Event evt, Type type)
        {
            if (type == null) return;
            var sourceInfo = GetSourceInfo(type);
            if (sourceInfo == null)
            {
                Debug.LogWarning($"Cannot find source script of \"{type.FullName}\" because it does not have \"{nameof(ProvideSourceInfoAttribute)}\" defined.");
                return;
            }
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            switch (evt.clickCount)
            {
                case 1: PingObject(monoScript); break;
                case 2: OpenAsset(monoScript, sourceInfo.LineNumber); break;
            }
        }

        private static Rect DrawTypeLabel(Rect position, Type type)
        {
            Button(position, GetTypeGUIContent(type), EditorStyles.objectField);
            return position;
        }

        private static Rect DrawTypePicker(Rect position)
        {
            Button(position, GUIContent.none, pickButtonStyle);
            return position;
        }

        private static Rect GetPickerArea(Rect position)
        {
            position.height = singleLineHeight - 2f;
            position.y += 1f;
            position.xMax -= 1f;
            position.xMin = position.xMax - singleLineHeight;
            return position;
        }

        private static void OpenMonoScript(ProvideSourceInfoAttribute sourceInfo)
        {
            if (sourceInfo != null)
            {
                var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
                OpenAsset(monoScript, sourceInfo.LineNumber);
            }
        }

        private static ProvideSourceInfoAttribute GetSourceInfo(Type type)
        {
            return type?.GetCustomAttribute<ProvideSourceInfoAttribute>();
        }

        public static GUIContent GetTypeGUIContent(Type type)
        {
            var label = type != null
                ? string.IsNullOrEmpty(type.Namespace)
                    ? type.Name
                    : $"{type.Name} ({type.Namespace})"
                : "None (Type)";
            var image = IconContent("FilterByType").image;
            return new GUIContent(label, image);
        }
    }
}
