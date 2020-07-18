using UnityEngine;

namespace CareBoo.Serially.Editor.Tests
{
    public class SerializableInstanceHolder<T> : ScriptableObject
    {
        public T Instance;
    }
}
