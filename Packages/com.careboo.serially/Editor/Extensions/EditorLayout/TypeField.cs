﻿using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;
using static UnityEditor.AssetDatabase;
using static UnityEditor.EditorGUIUtility;

namespace CareBoo.Serially.Editor
{
    public static partial class EditorGUIExtensions
    {
        private static class Styles
        {
            public static GUIStyle pickButton = "ObjectFieldButton";
            public static GUIStyle objectField = new GUIStyle("ObjectField") { richText = true };
        }

        public static Rect TypeField(
            Rect position,
            Type type,
            Type[] types,
            Action<Type> onSelect,
            Event customEvent = null) // Have to use this hack to get this to work...
        {
            var pickerArea = GetPickerArea(position);
            var evt = customEvent ?? Event.current;
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var eventType = customEvent == null
                ? evt.GetTypeForControl(controlId)
                : evt.rawType;
            if (eventType == EventType.MouseDown)
            {
                if (pickerArea.Contains(evt.mousePosition))
                    TypePickerWindow.ShowWindow(type, types, onSelect);
                else if (position.Contains(evt.mousePosition))
                    HandleTypeLabelClicked(evt, type);
            }
            DrawTypeLabel(position, type);
            DrawTypePicker(pickerArea);

            position.y += singleLineHeight + standardVerticalSpacing;
            return position;
        }

        public static MonoScript HandleTypeLabelClicked(Event evt, Type type)
        {
            if (type == null) return null;
            var sourceInfo = GetSourceInfo(type);
            if (sourceInfo == null)
            {
                Debug.LogWarning($"Cannot find source script of \"{type?.FullName ?? "Null"}\" because it doesn't have a \"{nameof(ProvideSourceInfoAttribute)}\".");
                return null;
            }
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            switch (evt.clickCount)
            {
                case 1: OnTypeLabelClickedOnce(sourceInfo); break;
                case 2: OnTypeLabelClickedTwice(sourceInfo); break;
            }
            if (monoScript == null)
                Debug.LogWarning($"Cannot find MonoScript at the path \"{sourceInfo.AssetPath}\".");
            return monoScript;
        }

        public static Action<ProvideSourceInfoAttribute> OnTypeLabelClickedOnce { get; set; } = sourceInfo =>
        {
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            PingObject(monoScript);
        };

        public static Action<ProvideSourceInfoAttribute> OnTypeLabelClickedTwice { get; set; } = sourceInfo =>
        {
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            OpenAsset(monoScript, sourceInfo.LineNumber);
        };

        public static Rect DrawTypeLabel(Rect position, Type type)
        {
            EditorStyles.objectField.richText = true;
            Button(position, GetTypeGUIContent(type), Styles.objectField);
            return position;
        }

        public static Rect DrawTypePicker(Rect position)
        {
            Button(position, GUIContent.none, Styles.pickButton);
            return position;
        }

        public static Rect GetPickerArea(Rect position)
        {
            position.height = singleLineHeight - 2f;
            position.y += 1f;
            position.xMax -= 1f;
            position.xMin = position.xMax - singleLineHeight;
            return position;
        }

        public static void OpenMonoScript(ProvideSourceInfoAttribute sourceInfo)
        {
            if (sourceInfo != null)
            {
                var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
                OpenAsset(monoScript, sourceInfo.LineNumber);
            }
        }

        public static ProvideSourceInfoAttribute GetSourceInfo(Type type)
        {
            return type?.GetCustomAttribute<ProvideSourceInfoAttribute>();
        }

        public static string GetTypeLabelString(Type type)
        {
            var label = type != null
                ? string.IsNullOrEmpty(type.Namespace)
                    ? type.Name
                    : $"{type.Name} <i>({type.Namespace})</i>"
                : "None <i>(Type)</i>";
            return label;
        }

        public static Texture GetTypeImage()
        {
            return IconContent("FilterByType").image;
        }

        public static GUIContent GetTypeGUIContent(Type type)
        {
            var label = GetTypeLabelString(type);
            var image = GetTypeImage();
            return new GUIContent(label, image);
        }
    }
}
