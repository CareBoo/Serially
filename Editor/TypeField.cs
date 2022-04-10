using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;
using static UnityEditor.AssetDatabase;
using static UnityEditor.EditorGUIUtility;

namespace CareBoo.Serially.Editor
{
    public struct TypeField
    {
        public static class Styles
        {
            public readonly static GUIStyle pickerButton = "ObjectFieldButton";
            public readonly static GUIStyle objectField = new GUIStyle("ObjectField") { richText = true };
        }

        public Rect Position { get; }
        public Type SelectedType { get; }
        public Lazy<IEnumerable<Type>> SelectableTypes { get; }
        public Action<Type> OnSelectType { get; }
        public GuiEvent CurrentGuiEvent { get; }

        public GUIContent LabelGuiContent { get; }
        public Rect PickerButtonArea { get; }
        public ProvideSourceInfoAttribute SourceInfo { get; }

        public static GUIContent GetLabelGuiContent(Type type)
        {
            var label = GetLabelString(type);
            var image = GetLabelImage();
            return new GUIContent(label, image);
        }

        public static Rect GetPickerButtonArea(Rect position)
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

        public static string GetLabelString(Type type)
        {
            var label = type != null
                ? string.IsNullOrEmpty(type.Namespace)
                    ? type.Name
                    : $"{type.Name} <i>({type.Namespace})</i>"
                : "None <i>(Type)</i>";
            return label;
        }

        public static Texture GetLabelImage()
        {
            return IconContent("FilterByType").image;
        }

        public TypeField(
            Rect position,
            Type selectedType,
            Lazy<IEnumerable<Type>> selectableTypes,
            Action<Type> onSelectType,
            GuiEvent? currentGuiEvent = null
            )
        {
            Position = position;
            SelectedType = selectedType;
            SelectableTypes = selectableTypes;
            OnSelectType = onSelectType;
            CurrentGuiEvent = currentGuiEvent ?? GuiEvent.FromCurrentUnityEvent;

            LabelGuiContent = GetLabelGuiContent(selectedType);
            PickerButtonArea = GetPickerButtonArea(position);
            SourceInfo = GetSourceInfo(selectedType);
        }

        public void DrawGui()
        {
            DrawLabel();
            DrawPickerButton();
            HandleCurrentEvent();
        }

        public void DrawLabel()
        {
            Button(Position, LabelGuiContent, Styles.objectField);
        }

        public void DrawPickerButton()
        {
            Button(PickerButtonArea, GUIContent.none, Styles.pickerButton);
        }

        public void HandleCurrentEvent()
        {
            if (Position.Contains(CurrentGuiEvent.MousePosition)
                && CurrentGuiEvent.Type == EventType.MouseDown)
                HandleMouseDown();
        }

        public void HandleMouseDown()
        {
            if (PickerButtonArea.Contains(CurrentGuiEvent.MousePosition)) HandlePickerButtonClicked();
            else HandleTypeLabelClicked();
        }

        public void HandlePickerButtonClicked()
        {
            TypePickerWindow.ShowWindow(SelectedType, SelectableTypes.Value, OnSelectType);
        }

        public MonoScript HandleTypeLabelClicked()
        {
            if (SourceInfo == null)
            {
                if (SelectedType != null)
                    Debug.LogWarning($"Cannot find source script of \"{SelectedType.FullName}\" because it doesn't have a \"{nameof(ProvideSourceInfoAttribute)}\".");
                return null;
            }
            var monoScript = LoadAssetAtPath<MonoScript>(SourceInfo.AssetPath);
            if (CurrentGuiEvent.ClickCount > 1)
                OpenAsset(monoScript, SourceInfo.LineNumber);
            else
                PingObject(monoScript);
            return monoScript;
        }
    }
}
