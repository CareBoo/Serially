using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CareBoo.Serially.Editor.UI
{
    public class NotifyClassChange : VisualElement
    {
        new public void AddToClassList(string className)
        {
            base.AddToClassList(className);
        }

        new public void RemoveFromClassList(string className)
        {
            base.RemoveFromClassList(className);
        }
    }
}
