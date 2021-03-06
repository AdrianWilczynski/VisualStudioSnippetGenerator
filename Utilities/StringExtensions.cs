using System.Text.RegularExpressions;

namespace VisualStudioSnippetGenerator.Utilities
{
    public static class StringExtensions
    {
        public static string ToSeparateWords(this string text)
            => string.Join(' ', Regex.Split(text, "(?<=[a-z])(?=[A-Z])"));

        public static string OnlyWordCharacters(this string text)
            => Regex.Replace(text, @"[^\w]+", "_");
    }
}