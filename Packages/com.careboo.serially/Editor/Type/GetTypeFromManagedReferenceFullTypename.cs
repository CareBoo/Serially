using System;

namespace CareBoo.Serially.Editor
{
    public static partial class TypeExtensions
    {
        // Taken from Unity cs reference: 
        // https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/ScriptAttributeGUI/ScriptAttributeUtility.cs
        public static bool GetTypeFromManagedReferenceFullTypename(
            string managedReferenceFullTypename,
            out Type type)
        {
            type = null;

            var parts = managedReferenceFullTypename.Split(' ');
            if (parts.Length == 2)
            {
                var assemblyPart = parts[0];
                var nsClassnamePart = parts[1];
                type = Type.GetType($"{nsClassnamePart}, {assemblyPart}");
            }

            return type != null;
        }
    }
}
