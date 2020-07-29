using System;
using System.Linq.Expressions;
using UnityEditor;

namespace CareBoo.Serially.Editor.Tests
{
    public class EditorEvent
    {
        private readonly Action action;

        public int CallCount { get; private set; } = 0;

        public EditorEvent(Action action)
        {
            this.action = action;
        }

        public void Invoke()
        {
            action?.Invoke();
            CallCount += 1;
        }

        public static implicit operator EditorEvent(Action action) => new EditorEvent(action);
        public static implicit operator Action(EditorEvent editorEvent) => editorEvent.action;
    }

    public class TestEditorWindow : EditorWindow
    {
        public EditorEvent awake;
        public EditorEvent onDestroy;
        public EditorEvent onFocus;
        public EditorEvent onGui;
        public EditorEvent onHierarchyChange;
        public EditorEvent onInspectorUpdate;
        public EditorEvent onLostFocus;
        public EditorEvent onProjectChange;
        public EditorEvent onSelectionChange;
        public EditorEvent update;
        public EditorEvent onDisable;
        public EditorEvent onEnable;

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

        void Handle(EditorEvent editorEvent) => editorEvent?.Invoke();

        public bool OnGUIInitialized()
        {
            return onGui != null && onGui.CallCount >= 4;
        }
    }
}
