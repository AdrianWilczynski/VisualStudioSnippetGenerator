using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VisualStudioSnippetGenerator.Services
{
    public class ReplacementService
    {
        public string ReplacementRegex(char delimeter)
            => $"{Regex.Escape(delimeter.ToString())}([^{Regex.Escape(delimeter.ToString())}]*){Regex.Escape(delimeter.ToString())}";

        public string ToReplacement(string identifier, char delimeter)
            => delimeter + identifier + delimeter;

        public string UpdateReplacements(string code, string oldIdentifier, string newIdentifier, char delimeter)
            => Regex.IsMatch(newIdentifier, $"^[^{Regex.Escape(delimeter.ToString())}]+$")
            ? code.Replace(ToReplacement(oldIdentifier, delimeter), ToReplacement(newIdentifier, delimeter))
            : code;

        public string UpdateReplacements(string code, char oldDelimeter, char newDelimeter)
        {
            foreach (var replacement in MatchReplacements(code, oldDelimeter))
            {
                code = code.Replace(
                    ToReplacement(replacement, oldDelimeter),
                    ToReplacement(replacement, newDelimeter));
            }

            return code;
        }

        public IEnumerable<string> MatchReplacements(string code, char delimeter)
            => Regex.Matches(code, ReplacementRegex(delimeter))
                .Select(m => m.Groups[1].Value)
                .Where(i => i.Length != 0);

        public string Escape(string code, char delimeter)
            => code.Replace(delimeter.ToString(), delimeter.ToString() + delimeter);

        public string Unescape(string code, char delimeter)
            => code.Replace(delimeter.ToString() + delimeter, delimeter.ToString());
    }
}