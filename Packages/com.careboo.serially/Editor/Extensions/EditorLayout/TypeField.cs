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
        public static partial class Styles
        {
            public readonly static GUIStyle pickButton = "ObjectFieldButton";
            public readonly static GUIStyle objectField = new GUIStyle("ObjectField") { richText = true };
        }

        public static Rect TypeField(
            Rect position,
            Type selectedType,
            Type[] selectableTypes,
            Action<Type> onSelect)
        {
            var fieldOptions = new TypeFieldOptions(position, selectedType, selectableTypes, onSelect);
            var evt = Event.current;
            var guiEvent = new GUIEvent(evt.type, evt.mousePosition, evt.clickCount);
            return TypeField(fieldOptions, guiEvent);
        }

        public static Rect TypeField(TypeFieldOptions fieldOptions, GUIEvent guiEvent)
        {
            DrawTypeLabel(fieldOptions.Position, fieldOptions.SelectedType);
            var typePickerPosition = DrawTypePicker(fieldOptions.Position);

            if (guiEvent.Type == EventType.MouseDown)
                HandleMouseDown(fieldOptions, typePickerPosition, guiEvent);

            return AddTypeFieldVertical(fieldOptions.Position);
        }

        public static void HandleMouseDown(TypeFieldOptions fieldOptions, Rect typePickerPosition, GUIEvent guiEvent)
        {
            var fieldPosition = fieldOptions.Position;

            if (typePickerPosition.Contains(guiEvent.MousePosition))
                HandleTypePickerButtonClicked(fieldOptions);
            else if (fieldPosition.Contains(guiEvent.MousePosition))
                HandleTypeLabelClicked(fieldOptions, guiEvent);
        }

        public static Rect AddTypeFieldVertical(Rect position)
        {
            position.y += singleLineHeight + standardVerticalSpacing;
            return position;
        }

        public static void HandleTypePickerButtonClicked(TypeFieldOptions fieldOptions)
        {
            TypePickerWindow.ShowWindow(fieldOptions.SelectedType, fieldOptions.SelectableTypes, fieldOptions.OnSelect);
        }

        public static MonoScript HandleTypeLabelClicked(TypeFieldOptions fieldOptions, GUIEvent guiEvent)
        {
            var selectedType = fieldOptions.SelectedType;
            if (selectedType == null) return null;
            var sourceInfo = GetSourceInfo(selectedType);
            if (sourceInfo == null)
            {
                Debug.LogWarning($"Cannot find source script of \"{selectedType?.FullName ?? "Null"}\" because it doesn't have a \"{nameof(ProvideSourceInfoAttribute)}\".");
                return null;
            }
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            switch (guiEvent.ClickCount)
            {
                case 1: OnTypeLabelClickedOnce(sourceInfo); break;
                case 2: OnTypeLabelClickedTwice(sourceInfo); break;
            }
            return monoScript;
        }

        public static void OnTypeLabelClickedOnce(ProvideSourceInfoAttribute sourceInfo)
        {
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            PingObject(monoScript);
        }

        public static void OnTypeLabelClickedTwice(ProvideSourceInfoAttribute sourceInfo)
        {
            var monoScript = LoadAssetAtPath<MonoScript>(sourceInfo.AssetPath);
            OpenAsset(monoScript, sourceInfo.LineNumber);
        }

        public static Rect DrawTypeLabel(Rect position, Type type)
        {
            EditorStyles.objectField.richText = true;
            Button(position, GetTypeGUIContent(type), Styles.objectField);
            return position;
        }

        public static Rect DrawTypePicker(Rect position)
        {
            position = GetTypePickerButtonPosition(position);
            Button(position, GUIContent.none, Styles.pickButton);
            return position;
        }

        public static Rect GetTypePickerButtonPosition(Rect position)
        {
            position.height = singleLineHeight - 2f;
            position.y += 1f;
            position.xMax -= 1f;
            position.xMin = position.xMax - singleLineHeight;
            return position;
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
