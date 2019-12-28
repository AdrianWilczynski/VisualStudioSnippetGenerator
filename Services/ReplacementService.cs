using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VisualStudioSnippetGenerator.Services
{
    public class ReplacementService
    {
        public const string ReplacementRegex = @"\$([^\s$]*)\$";

        public string ToReplacement(string identifier) => $"${identifier}$";

        public string UpdateReplacements(string code, string oldIdentifier, string newIdentifier)
            => Regex.IsMatch(newIdentifier, @"^[^\s$]+$")
            ? code.Replace(ToReplacement(oldIdentifier), ToReplacement(newIdentifier))
            : code;

        public IEnumerable<string> MatchReplacements(string code)
            => Regex
                .Matches(code, ReplacementRegex)
                .Select(m => m.Groups[1].Value)
                .Where(i => i.Length != 0);
    }
}