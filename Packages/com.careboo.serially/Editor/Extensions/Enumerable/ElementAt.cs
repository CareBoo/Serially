using System;
using System.Collections;

namespace CareBoo.Serially.Editor
{
    public static partial class EnumerableExtensions
    {
        public static object ElementAtOrDefault(this IEnumerable sequence, int index)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            foreach (var element in sequence)
            {
                if (index == 0) return element;
                index--;
            }
            return null;
        }
    }
}
