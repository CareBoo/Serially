using System;
using System.Runtime.InteropServices;

namespace CareBoo.Serially.Samples
{
    public interface IPet
    {
        string Name { get; set; }
        string Noise { get; }
    }

    [ProvideSourceInfo]
    [Serializable]
    [Guid("97a74ff6-2b61-420b-ad92-c01668fefc28")]
    public class Cat : IPet
    {
        public string Name;

        public bool IsPurring;

        string IPet.Name
        {
            get => Name;
            set => Name = value;
        }

        string IPet.Noise => "Meow";
    }

    [ProvideSourceInfo]
    [Serializable]
    [Guid("3e959821-17ed-4cc6-82f8-d629536be6ed")]
    public class Dog : IPet
    {
        public string Name;

        public bool IsWaggingTail;

        string IPet.Name
        {
            get => Name;
            set => Name = value;
        }

        string IPet.Noise => "Woof";
    }

    [ProvideSourceInfo]
    [Serializable]
    [Guid("b19cb907-dd97-42cd-94fc-c537382f971e")]
    public class Dalmation : Dog
    {
        public int NumberOfSpots;
    }
}
