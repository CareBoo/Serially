using UnityEngine;

namespace CareBoo.Serially.Editor
{
    public struct GUIEvent
    {
        public EventType Type { get; }
        public Vector2 MousePosition { get; }
        public int ClickCount { get; }

        public GUIEvent(
            EventType type,
            Vector2 mousePosition,
            int clickCount
        )
        {
            Type = type;
            MousePosition = mousePosition;
            ClickCount = clickCount;
        }
    }
}
