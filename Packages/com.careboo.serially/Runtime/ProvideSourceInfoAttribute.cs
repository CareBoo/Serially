using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CareBoo.Serially
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    public class ProvideSourceInfoAttribute : Attribute
    {
        private readonly string absoluteFilePath;
        private string assetPath;

        public string Member { get; }
        public int LineNumber { get; }
        public string AssetPath
        {
            get
            {
                if (assetPath == null)
                {
                    assetPath = "Assets" + absoluteFilePath.Substring(Application.dataPath.Length);
                }
                return assetPath;
            }
        }

        public ProvideSourceInfoAttribute(
            [CallerMemberName] string member = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Member = member;
            absoluteFilePath = filePath;
            LineNumber = lineNumber;
        }
    }
}
