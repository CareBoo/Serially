using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CareBoo.Serially.Editor
{
    public class TypePickerWindow : EditorWindow
    {
        public const string ContentContainer = "content-container";
        public const string TypeIcon = "type-icon";
        public const string TypeNameLabel = "type-name";
        public const string TypeNamespaceLabel = "type-namespace";
        public const string TypeListName = "type-list";
        public const string SearchFieldName = "search-field";
        public const string WindowAssetPath = "type_picker_window";
        public const string ItemAssetPath = "type_picker_item";

        public static readonly Regex TypeLabelRegex = new Regex(
            @"^(?<name>\w+\s?)(\<i\>(?<namespace>[\w\.\(\)]*)\</i\>)?",
            RegexOptions.Compiled
        );

        private VisualTreeAsset itemVisualTreeAsset;
        private Type preselected;
        private IEnumerable<Type> types;
        private List<Type> searchedTypes;
        private Action<Type> onSelected;
        private string searchValue;
        private ListView listView;

        private ToolbarSearchField searchField;

        private ToolbarSearchField SearchField => searchField
            ?? (searchField = rootVisualElement.Q<ToolbarSearchField>(name: SearchFieldName));

        public static TypePickerWindow ShowWindow(
            Type preselected,
            IEnumerable<Type> types,
            Action<Type> onSelected,
            string title = null
            )
        {
            var window = GetWindow<TypePickerWindow>(utility: true, title: title ?? "Select Type");
            window.InitData(preselected, types, onSelected);
            window.ShowAuxWindow();
            return window;
        }

        public void InitData(Type preselected, IEnumerable<Type> types, Action<Type> onSelected)
        {
            this.preselected = preselected;
            this.types = types;
            this.onSelected = onSelected;
            UpdateTypeSearch(null);
            SearchField.RegisterValueChangedCallback(UpdateTypeSearch);
        }

        void OnEnable()
        {
            InitWindow();
            InitListView();
            listView.onItemChosen += OnItemChosen;
        }

        void OnDisable()
        {
            listView.onItemChosen -= OnItemChosen;
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

        public VisualElement MakeItem() =>
            itemVisualTreeAsset.CloneTree()
            .Q<VisualElement>(name: ContentContainer);

        public void BindItem(VisualElement element, int index)
        {
            var type = searchedTypes.ElementAt(index);
            var text = TypeField.GetLabelString(type);
            var groups = TypeLabelRegex.Match(text).Groups;

            var typeNameGroup = groups["name"];
            var typeNameLabel = element.Q<Label>(name: TypeNameLabel);
            typeNameLabel.text = typeNameGroup.Success ? typeNameGroup.Value : string.Empty;

            var typeNamespaceGroup = groups["namespace"];
            var typeNamespaceLabel = element.Q<Label>(name: TypeNamespaceLabel);
            typeNamespaceLabel.text = typeNamespaceGroup.Success ? typeNamespaceGroup.Value : string.Empty;

            var typeIcon = element.Q<VisualElement>(name: TypeIcon);
            typeIcon.style.backgroundImage = (Texture2D)TypeField.GetLabelImage();
        }

        public void OnItemChosen(object item)
        {
            onSelected?.Invoke((Type)item);
            Close();
        }

        public void UpdateTypeSearch(ChangeEvent<string> stringChangeEvent)
        {
            searchValue = stringChangeEvent?.newValue;
            searchedTypes = types.Where(IsInSearch).Prepend(null).ToList();
            listView.itemsSource = searchedTypes;
            listView.selectedIndex = searchedTypes.IndexOf(preselected);
            listView.Refresh();
        }

        public bool IsInSearch(Type type)
        {
            return string.IsNullOrEmpty(searchValue)
                || type.AssemblyQualifiedName.Contains(searchValue);
        }
    }
}
