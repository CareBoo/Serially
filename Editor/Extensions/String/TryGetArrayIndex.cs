using System.Text.RegularExpressions;

namespace CareBoo.Serially.Editor
{
    public static partial class StringExtensions
    {

        private const string IndexKey = "index";

        private static readonly Regex ArrayRegex = new Regex(
            $"\\[(?<{IndexKey}>\\d+)\\]",
            RegexOptions.Compiled);

        public static bool TryGetArrayIndex(this string propertyStr, out int index)
        {
            index = -1;
            var arrayMatch = ArrayRegex.Match(propertyStr);
            if (!arrayMatch.Success) return false;

            index = int.Parse(arrayMatch.Groups[IndexKey].Value);
            return true;
        }
    }
}
