using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static CareBoo.Serially.Editor.EditorGUIExtensions;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace CareBoo.Serially.Editor
{
    public class TypePickerWindow : EditorWindow
    {
        public const string TypeItemName = "type-item";
        public const string TypeImageName = "type-image";
        public const string TypeLabelName = "type-label";
        public const string TypeListName = "type-list";
        public const string SearchFieldName = "search-field";
        public const string WindowAssetPath = "type_picker_window";
        public const string ItemAssetPath = "type_picker_item";

        private VisualTreeAsset itemVisualTreeAsset;

        private Type selected;
        private IEnumerable<Type> types;
        private List<Type> searchedTypes;
        private Action<Type> onSelected;
        private string searchValue;
        private ListView listView;

        private ToolbarSearchField searchField;

        private ToolbarSearchField SearchField => searchField
            ?? (searchField = rootVisualElement.Q<ToolbarSearchField>(name: SearchFieldName));

        public static TypePickerWindow ShowWindow(
            Type selected,
            IEnumerable<Type> types,
            Action<Type> onSelected,
            string title = null
            )
        {
            var window = (TypePickerWindow)CreateInstance(typeof(TypePickerWindow));
            window.InitData(selected, types, onSelected, title);
            window.ShowAuxWindow();
            return window;
        }

        public void InitData(Type selected, IEnumerable<Type> types, Action<Type> onSelected, string title = null)
        {
            this.selected = selected;
            this.types = types;
            this.onSelected = onSelected;
            titleContent = new GUIContent(title ?? "Select Type");
            UpdateTypeSearch(null);
            SearchField.RegisterValueChangedCallback(UpdateTypeSearch);
        }

        private void OnEnable()
        {
            InitWindow();
            InitListView();
            listView.onItemChosen += OnItemChosen;
            listView.onSelectionChanged += OnSelectionChanged;
        }

        private void OnDisable()
        {
            listView.onItemChosen -= OnItemChosen;
            listView.onSelectionChanged -= OnSelectionChanged;
        }

        private void InitWindow()
        {
            var windowVisualTreeAsset = Resources.Load<VisualTreeAsset>(WindowAssetPath);
            windowVisualTreeAsset.CloneTree(rootVisualElement);
            itemVisualTreeAsset = Resources.Load<VisualTreeAsset>(ItemAssetPath);
        }

        private ListView InitListView()
        {
            listView = rootVisualElement.Q<ListView>(name: TypeListName);
            listView.selectionType = SelectionType.Single;
            listView.itemsSource = searchedTypes;
            listView.makeItem = MakeItem;
            listView.bindItem = BindItem;
            return listView;
        }

        private VisualElement MakeItem()
        {
            var container = itemVisualTreeAsset.CloneTree();
            var element = container.Q<VisualElement>(name: TypeItemName);
            var imageElement = element.Q<VisualElement>(name: TypeImageName);
            var image = (Texture2D)GetTypeImage();
            imageElement.style.backgroundImage = new StyleBackground(image);
            return element;
        }

        private void BindItem(VisualElement element, int index)
        {
            var type = searchedTypes.ElementAt(index);
            var label = element.Q<Label>(name: TypeLabelName);
            label.text = GetTypeLabelString(type);
        }

        private void OnItemChosen(object item)
        {
            onSelected?.Invoke((Type)item);
            Close();
        }

        private void OnSelectionChanged(List<object> _)
        {
            onSelected?.Invoke((Type)listView.selectedItem);
        }

        private void UpdateTypeSearch(ChangeEvent<string> stringChangeEvent)
        {
            searchValue = stringChangeEvent?.newValue;
            searchedTypes = types.Where(IsInSearch).Prepend(null).ToList();
            listView.itemsSource = searchedTypes;
            listView.selectedIndex = searchedTypes.IndexOf(selected);
        }

        private bool IsInSearch(Type type)
        {
            return string.IsNullOrEmpty(searchValue)
                || type.AssemblyQualifiedName.Contains(searchValue);
        }
    }
}
