using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VisualStudioSnippetGenerator.Models;

namespace VisualStudioSnippetGenerator.Services
{
    public class ReplacementService
    {
        public const string IdentifierRegex = @"([^\s$]+)";

        public string ReplacementRegex(string identifierRegex)
            => $@"(?<!\$)\${identifierRegex}\$(?!\$)";

        public string ToReplacement(string identifier)
            => $"${identifier}$";

        public bool IsReplacementIdentifier(string identifier)
            => Regex.IsMatch(identifier, $"^{IdentifierRegex}$");

        public string UpdateReplacements(string body, string oldIdentifier, string newIdentifier)
            => Regex.Replace(body, ReplacementRegex(Regex.Escape(oldIdentifier)), ToReplacement(newIdentifier));

        public IEnumerable<string> MatchReplacements(string body)
            => Regex
                .Matches(body, ReplacementRegex(IdentifierRegex))
                .Select(m => m.Groups[1].Value);

        public IEnumerable<Literal> MapReplacementsToLiterals(IEnumerable<string> replacements, IEnumerable<Literal> literals)
            => literals
                .Where(l => replacements.Any(r => r == l.Identifier) || l.Touched)
                .Concat(replacements
                    .Where(r => !Constants.ReservedKeywords.All.Contains(r) && !literals.Any(l => l.Identifier == r))
                    .Select(r => new Literal(r)));
    }
}