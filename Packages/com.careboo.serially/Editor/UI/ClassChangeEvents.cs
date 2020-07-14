using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CareBoo.Serially.Editor.UI
{
    public interface IClassChangeEvent
    {
        string ClassDiff { get; }
    }

    public abstract class ClassChangeEvent<T> : EventBase<T>, IClassChangeEvent where T : ClassChangeEvent<T>, new()
    {
        public string ClassDiff { get; protected set; } = null;

        public static T GetPooled(IEventHandler target, string classDiff)
        {
            T e = GetPooled();
            e.target = target;
            e.ClassDiff = classDiff;
            return e;
        }
    }

    public class AddToClassListEvent : ClassChangeEvent<AddToClassListEvent> { }

    public class RemoveFromClassListEvent : ClassChangeEvent<RemoveFromClassListEvent> { }
}