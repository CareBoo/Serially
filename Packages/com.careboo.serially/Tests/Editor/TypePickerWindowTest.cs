using System;
using NUnit.Framework;
using UnityEditor;

namespace CareBoo.Serially.Editor.Tests
{
    class TypePickerWindowTest
    {
        class A { }

        [TearDown]
        public void CloseTypePickerWindows()
        {
            while (EditorWindow.HasOpenInstances<TypePickerWindow>())
                EditorWindow.GetWindow<TypePickerWindow>().Close();
        }

        [Test]
        [Timeout(5000)]
        public void OnItemChosenShouldSelectType()
        {
            bool onSelectCalled = false;
            void OnSelect(Type type)
            {
                if (type == typeof(A))
                    onSelectCalled = true;
            }

            var typePickerWindow = TypePickerWindow.ShowWindow(typeof(A), new[] { typeof(A) }, OnSelect);
            typePickerWindow.OnItemChosen(typeof(A));
            Assert.IsTrue(onSelectCalled);
        }

        [Test]
        [Timeout(5000)]
        public void OnItemChosenShouldCloseWindow()
        {
            var typePickerWindow = TypePickerWindow.ShowWindow(typeof(A), new[] { typeof(A) }, null);
            typePickerWindow.OnItemChosen(typeof(A));
            Assert.IsFalse(EditorWindow.HasOpenInstances<TypePickerWindow>());
        }

        public static object[] TypeLabelRegexCases = new object[]
        {
            new object[] { "Class", "Class", "" },
            new object[] { "Class <i>(Namespace)</i>", "Class ", "(Namespace)"},
        };

        [Test]
        [Timeout(5000)]
        [TestCaseSource(nameof(TypeLabelRegexCases))]
        public void TypeLabelRegexShouldExtractNameAndNamespace(string input, string expectedName, string expectedNamespace)
        {
            var groups = TypePickerWindow.TypeLabelRegex.Match(input).Groups;
            var actualName = groups["name"].Value;
            var actualNamespace = groups["namespace"].Value;
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedNamespace, actualNamespace);
        }
    }
}
