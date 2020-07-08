using System.Linq;

namespace CareBoo.Serially.Editor
{
    public static partial class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            var splitInput = input.Select(SeparateUpperCaseWithUnderscore);
            string result = string.Concat(splitInput).ToLower();
            return result;
        }

        private static string SeparateUpperCaseWithUnderscore(char element, int index)
        {
            var separator = index > 0 && char.IsUpper(element) ? "_" : "";
            return $"{separator}{element}";
        }
    }
}
