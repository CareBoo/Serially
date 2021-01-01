using UnityEditor;
using UnityEngine;

namespace CareBoo.Serially.Editor.Tests
{
    public class TimeoutYieldInstruction : CustomYieldInstruction
    {
        readonly double startTime;
        readonly int ms;
        readonly CustomYieldInstruction yieldInstruction;

        public TimeoutYieldInstruction(int ms, CustomYieldInstruction yieldInstruction)
        {
            startTime = EditorApplication.timeSinceStartup;
            this.ms = ms;
            this.yieldInstruction = yieldInstruction;
        }

        public override bool keepWaiting
        {
            get
            {
                return (EditorApplication.timeSinceStartup - startTime) * 1000 < ms
                && yieldInstruction.keepWaiting;
            }
        }
    }

    public static class CustomYieldInstructionExtensions
    {
        public static TimeoutYieldInstruction OrTimeout(this CustomYieldInstruction yieldInstruction, int ms)
        {
            return new TimeoutYieldInstruction(ms, yieldInstruction);
        }
    }
}
