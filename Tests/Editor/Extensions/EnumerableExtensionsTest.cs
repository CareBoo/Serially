using System;
using System.Collections;
using System.Text.RegularExpressions;
using CareBoo.Serially.Editor;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CareBoo.Serially.Editor.Tests
{
    class EnumerableExtensionsTest
    {
        [Test]
        public void ElementAtOrDefaultShouldReturnValueWhenGivenIndexWithinDomain()
        {
            object expected = "hello";
            var enumerable = (IEnumerable)new string[2] { "not hello", "hello" };
            var actual = enumerable.ElementAtOrDefault(1);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ElementAtOrDefaultShouldReturnNullWhenGivenIndexOutsideDomain()
        {
            object expected = null;
            var enumerable = (IEnumerable)(new int[1]);
            var actual = enumerable.ElementAtOrDefault(2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ElementAtOrDefaultShouldThrowErrorWhenUsingNullSequence()
        {
            LogAssert.Expect(LogType.Exception, new Regex(nameof(ArgumentNullException)));
            IEnumerable enumerable = null;
            try
            {
                var element = enumerable.ElementAtOrDefault(1);
            }
            catch (ArgumentNullException ex)
            {
                Debug.LogException(ex);
            }
        }

        [Test]
        public void SkipLastShouldThrowExceptionWhenUsingNullSequence()
        {
            LogAssert.Expect(LogType.Exception, new Regex(nameof(ArgumentNullException)));
            IEnumerable sequence = null;
            try
            {
                var element = sequence.ElementAtOrDefault(1);
            }
            catch (ArgumentNullException ex)
            {
                Debug.LogException(ex);
            }
        }

        [Test]
        public void SkipLastWithZeroCountReturnsSequence()
        {
            var expected = new[] { 1, 2, 3 };
            var actual = expected.SkipLast(0);
            Assert.AreEqual(expected, actual);
        }
    }
}
