using System;
using UnityEngine;

namespace CareBoo.Serially.Editor.Tests
{
    public class WaitWhile : CustomYieldInstruction
    {
        public Func<bool> Condition { get; }

        public WaitWhile(Func<bool> condition)
        {
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override bool keepWaiting => Condition.Invoke();
    }
}
