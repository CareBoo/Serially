using UnityEngine;
using UnityEngine.TestTools;
using System;
using UnityEngine.Events;

namespace CareBoo.Serially.Editor.Tests
{
    [ExecuteInEditMode]
    public class OnGUIMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        private UnityAction onGui;

        public bool IsTestFinished { get; protected set; }

        public OnGUIMonoBehaviourTest Initialize(UnityAction onGui)
        {
            this.onGui = onGui;
            return this;
        }

        void OnGUI()
        {
            if (onGui != null)
            {
                IsTestFinished = true;
                onGui.Invoke();
            }
        }
    }

    public class OnGUITest : MonoBehaviourTest<OnGUIMonoBehaviourTest>
    {
        public OnGUITest(UnityAction onGui) : base(false)
        {
            component.Initialize(onGui);
        }
    }
}
