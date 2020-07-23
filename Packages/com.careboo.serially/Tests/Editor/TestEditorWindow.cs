using System;
using UnityEditor;

namespace CareBoo.Serially.Editor.Tests
{
    public class TestEditorWindow : EditorWindow
    {
        public Action awake;
        public Action onDestroy;
        public Action onFocus;
        public Action onGui;
        public Action onHierarchyChange;
        public Action onInspectorUpdate;
        public Action onLostFocus;
        public Action onProjectChange;
        public Action onSelectionChange;
        public Action update;
        public Action onDisable;
        public Action onEnable;

        void Awake() => Handle(awake);
        void OnDestroy() => Handle(onDestroy);
        void OnFocus() => Handle(onFocus);
        void OnGUI() => Handle(onGui);
        void OnHierarchyChange() => Handle(onHierarchyChange);
        void OnInspectorUpdate() => Handle(onInspectorUpdate);
        void OnLostFocus() => Handle(onLostFocus);
        void OnProjectChange() => Handle(onProjectChange);
        void OnSelectionChange() => Handle(onSelectionChange);
        void Update() => Handle(update);
        void OnDisable() => Handle(onDisable);
        void OnEnable() => Handle(onEnable);

        void Handle(Action action) => action?.Invoke();

    }
}
