using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VisualStudioSnippetGenerator.Services
{
    public class ReplacementService
    {
        public const string IdentifierRegex = @"([^\s$]+)";

        public string ReplacementRegex(string identifierRegex)
            => $@"(?<!\$)\${identifierRegex}\$(?!\$)";

        public string ToReplacement(string identifier)
            => $"${identifier}$";

        public bool IsIdentifier(string identifier)
            => Regex.IsMatch(identifier, $"^{IdentifierRegex}$");

        public string UpdateReplacements(string code, string oldIdentifier, string newIdentifier)
        {
            if (!IsIdentifier(newIdentifier))
            {
                return code;
            }

            return Regex.Replace(code, ReplacementRegex(Regex.Escape(oldIdentifier)), ToReplacement(newIdentifier));
        }

        public IEnumerable<string> MatchReplacements(string code)
            => Regex
                .Matches(code, ReplacementRegex(IdentifierRegex))
                .Select(m => m.Groups[1].Value);
    }
}