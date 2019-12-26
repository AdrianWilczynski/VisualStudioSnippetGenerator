using System.Text.RegularExpressions;

namespace VisualStudioSnippetGenerator.Extensions
{
    public static class StringExtensions
    {
        public static string ToSeparateWords(this string text)
            => string.Join(' ', Regex.Split(text, "(?<=[a-z])(?=[A-Z])"));
    }
}