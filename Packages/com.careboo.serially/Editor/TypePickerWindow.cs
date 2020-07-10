using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace CareBoo.Serially.Editor
{
    public class TypePickerWindow : EditorWindow
    {
        private const string TypeListName = "type-list";
        private const string SearchFieldName = "search-field";


        private Type selected;
        private Type[] types;
        private Type[] searchedTypes;
        private Action<Type> onSelected;
        private string searchValue;

        private ToolbarSearchField searchField;
        private ToolbarSearchField SearchField =>
            searchField
            ?? (searchField = rootVisualElement.Q<ToolbarSearchField>(name: SearchFieldName));

        private ListView listView;
        public ListView ListView =>
            listView
            ?? (listView = rootVisualElement.Q<ListView>(name: TypeListName));

        public static TypePickerWindow ShowWindow(
            Type selected,
            Type[] types,
            Action<Type> onSelected,
            string title = null
        )
        {
            var window = (TypePickerWindow)CreateInstance(typeof(TypePickerWindow));
            window.Init(selected, types, onSelected, title);
            window.ShowAuxWindow();
            return window;
        }

        public void Init(Type selected, Type[] types, Action<Type> onSelected, string title = null)
        {
            this.selected = selected;
            this.types = types;
            this.onSelected = onSelected;
            titleContent = new GUIContent(title ?? "Select Type");
            UpdateTypeSearch(null);
            ResetListContent();
            SearchField.RegisterValueChangedCallback(UpdateTypeSearch);
        }

        private void OnEnable()
        {
            AddWindowVisualElement();
        }

        private void AddWindowVisualElement()
        {
            var assetPath = nameof(TypePickerWindow).ToSnakeCase();
            var windowVisualTreeAsset = Resources.Load<VisualTreeAsset>(assetPath);
            windowVisualTreeAsset.CloneTree(rootVisualElement);
        }

        private void ResetListContent()
        {
            ListView.Clear();
            AddTypeElement(null);
            foreach (var type in searchedTypes)
                AddTypeElement(type);
        }

        private void AddTypeElement(Type type)
        {
            var element = new TypePickerListElement(type, Select);
            ListView.Add(element);
            element.SetHighlight(type == selected);
        }

        private void UpdateTypeSearch(ChangeEvent<string> stringChangeEvent)
        {
            searchValue = stringChangeEvent?.newValue;
            searchedTypes = string.IsNullOrEmpty(searchValue)
                ? types
                : types.Where(IsInSearch).ToArray();
            ResetListContent();
        }

        private bool IsInSearch(Type type)
        {
            return type.AssemblyQualifiedName.Contains(searchValue);
        }

        private void Select(Type type, MouseDownEvent evt)
        {
            selected = type;
            onSelected?.Invoke(type);
            if (evt.clickCount >= 2)
                Close();
        }
    }
}
