using System.Text.RegularExpressions;

namespace CareBoo.Serially.Editor
{
    public static partial class StringExtensions
    {
        private const string NameKey = "name";

        private const string IndexKey = "index";

        private static readonly Regex ArrayRegex = new Regex(
            $"(?<{NameKey}>\\w+)\\[(?<{IndexKey}>\\d+)\\]",
            RegexOptions.Compiled);

        public static bool TryGetArrayIndex(this string propertyStr, out string name, out int index)
        {
            name = propertyStr; index = -1;
            var arrayMatch = ArrayRegex.Match(propertyStr);
            if (!arrayMatch.Success) return false;

            name = arrayMatch.Groups[NameKey].Value;
            index = int.Parse(arrayMatch.Groups[IndexKey].Value);
            return true;
        }
    }
}
