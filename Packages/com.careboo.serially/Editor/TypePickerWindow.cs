﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static CareBoo.Serially.Editor.EditorGUIExtensions;

namespace CareBoo.Serially.Editor
{
    public class TypePickerWindow : EditorWindow
    {
        private static class Styles
        {
            public static readonly GUIStyle selected = "selectionRect";
            public static readonly GUIStyle unselected = "ObjectPickerSmallStatus";
        }

        private const int MaxResults = 100;

        private Type selected;
        private Type[] types;
        private Type[] searchedTypes;
        private Action<Type> onSelected;
        private string searchValue;
        private Vector2 scrollPosition;

        public static TypePickerWindow ShowWindow(
            Type selected,
            Type[] types,
            Action<Type> onSelected,
            string title = null
        )
        {
            var window = (TypePickerWindow)CreateInstance(typeof(TypePickerWindow));
            window.types = types;
            window.onSelected = onSelected;
            window.selected = selected;
            window.UpdateTypeSearch(null);
            window.titleContent = new GUIContent(title ?? "Select Type");
            window.ShowAuxWindow();
            return window;
        }

        private void OnGUI()
        {
            DrawMaxResultsWarningLayout();
            DrawSearchFieldLayout();

            scrollPosition = BeginScrollView(scrollPosition);
            DrawSelectableTypeLayout(null);
            foreach (var type in searchedTypes.Take(MaxResults))
                DrawSelectableTypeLayout(type);
            EndScrollView();
        }

        private void DrawMaxResultsWarningLayout()
        {
            if (searchedTypes.Length > MaxResults)
                LabelField(
                    $"only showing the first {MaxResults} of {searchedTypes.Length} results...",
                    EditorStyles.centeredGreyMiniLabel
                    );
        }

        private void DrawSearchFieldLayout()
        {
            var newSearchValue = DelayedTextField(searchValue, EditorStyles.toolbarSearchField);
            if (newSearchValue != searchValue) UpdateTypeSearch(newSearchValue);
        }

        private void DrawSelectableTypeLayout(Type type)
        {
            var buttonStyle = selected == type
                ? Styles.selected
                : Styles.unselected;

            if (GUILayout.Button(GetTypeGUIContent(type), buttonStyle))
                Select(type);
        }

        private void UpdateTypeSearch(string newSearchValue)
        {
            searchValue = newSearchValue;
            searchedTypes = string.IsNullOrEmpty(searchValue)
                ? types
                : types.Where(IsInSearch).ToArray();
        }

        private bool IsInSearch(Type type)
        {
            return type.AssemblyQualifiedName.Contains(searchValue);
        }

        private void Select(Type type)
        {
            selected = type;
            onSelected?.Invoke(type);
        }
    }
}