using UnityEngine;

namespace CareBoo.Serially.Editor
{
    public struct GuiEvent
    {
        public EventType Type { get; }
        public Vector2 MousePosition { get; }
        public int ClickCount { get; }

        public GuiEvent(
            EventType type,
            Vector2 mousePosition,
            int clickCount
        )
        {
            Type = type;
            MousePosition = mousePosition;
            ClickCount = clickCount;
        }

        public static GuiEvent FromCurrentUnityEvent
        {
            get
            {
                var evt = Event.current;
                return new GuiEvent(evt.type, evt.mousePosition, evt.clickCount);
            }
        }
    }
}
