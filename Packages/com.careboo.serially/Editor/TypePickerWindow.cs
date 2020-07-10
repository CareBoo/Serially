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

        private TypePickerListElement selected;
        private Type preselectedType;
        private Type[] types;
        private Type[] searchedTypes;
        private Action<Type> onSelected;
        private string searchValue;

        public Type SelectedType =>
            selected?.Type ?? preselectedType;

        private ToolbarSearchField searchField;
        private ToolbarSearchField SearchField =>
            searchField
            ?? (searchField = rootVisualElement.Q<ToolbarSearchField>(name: SearchFieldName));

        private ListView listView;
        public ListView ListView =>
            listView
            ?? (listView = rootVisualElement.Q<ListView>(name: TypeListName));

        public static TypePickerWindow ShowWindow(
            Type preselectedType,
            Type[] types,
            Action<Type> onSelected,
            string title = null
            )
        {
            var window = (TypePickerWindow)CreateInstance(typeof(TypePickerWindow));
            window.Init(preselectedType, types, onSelected, title);
            window.ShowAuxWindow();
            return window;
        }

        public void Init(Type preselectedType, Type[] types, Action<Type> onSelected, string title = null)
        {
            this.preselectedType = preselectedType;
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
            element.SetHighlight(type == SelectedType);
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

        private void Select(TypePickerListElement element, MouseDownEvent evt)
        {
            selected?.SetHighlight(false);
            selected = element;
            selected?.SetHighlight(true);
            onSelected?.Invoke(selected?.Type);
            if (evt.clickCount >= 2)
                Close();
        }
    }
}
