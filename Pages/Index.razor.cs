using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VisualStudioSnippetGenerator.ViewModels;
using VisualStudioSnippetGenerator.Services;
using VisualStudioSnippetGenerator.Models;

namespace VisualStudioSnippetGenerator.Pages
{
    public partial class Index
    {
        public const string ReplacementRegex = @"(?<!\$)\$([^\s$]+)\$(?!\$)";

        private string _body = string.Empty;
        private string? _description;
        private string? _author;
        private string _language = string.Empty;
        private string _shortcut = string.Empty;
        private string _title = string.Empty;

        private readonly SnippetSerializer _snippetSerializer;

        public Index()
        {
            _snippetSerializer = new SnippetSerializer();

            SyncWithSnippetText();
        }

        public string Body
        {
            get => _body;
            set
            {
                _body = value;
                SyncBodyWithLiterals();
                SyncWithSnippetText();
            }
        }

        public List<LiteralViewModel> Literals { get; set; } = new List<LiteralViewModel>();

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SyncWithSnippetText();
            }
        }

        public string Shortcut
        {
            get => _shortcut;
            set
            {
                _shortcut = value;
                SyncWithSnippetText();
            }
        }

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                SyncWithSnippetText();
            }
        }

        public string? Description
        {
            get => string.IsNullOrWhiteSpace(_description) ? null : _description;
            set
            {
                _description = value;
                SyncWithSnippetText();
            }
        }

        public string? Author
        {
            get => string.IsNullOrWhiteSpace(_author) ? null : _author;
            set
            {
                _author = value;
                SyncWithSnippetText();
            }
        }

        public string SnippetText { get; set; } = string.Empty;

        public void OnLiteralIdInput(LiteralViewModel literal, string newValue)
        {
            if (IsReplacement(AsReplacement(newValue)) || newValue.Length == 0)
            {
                SyncLiteralIdWithBody(literal.Id, newValue);
            }

            literal.Id = newValue;
            SyncWithSnippetText();
        }

        public void OnLiteralDefaultValueInput(LiteralViewModel literal, string newValue)
        {
            literal.DefaultValue = newValue;
            SyncWithSnippetText();
        }

        public void OnRemoveLiteralClick(LiteralViewModel literal)
        {
            Literals.Remove(literal);

            SyncWithSnippetText();
        }

        public void SyncBodyWithLiterals()
            => Literals = MapReplacementsToLiterals(MatchReplacements(_body), Literals);

        public void SyncLiteralIdWithBody(string oldValue, string newValue)
            => _body = Regex.Replace(_body, $@"(?<!\$)\${Regex.Escape(oldValue)}\$(?!\$)", AsReplacement(newValue));

        public void SyncWithSnippetText()
            => SnippetText = _snippetSerializer.Serialize(
                    new VisualStudioSnippet(Title, Shortcut, Language, Body, Description, Author,
                        Literals.Select(l => new Literal(l.Id, l.DefaultValue))));

        public static string AsReplacement(string id)
            => $"${id}$";

        public static bool IsReplacement(string replacement)
            => Regex.IsMatch(replacement, $"^{ReplacementRegex}$");

        public static IEnumerable<string> MatchReplacements(string body)
            => Regex
                .Matches(body, ReplacementRegex)
                .Select(m => m.Groups[1].Value);

        public static List<LiteralViewModel> MapReplacementsToLiterals(
            IEnumerable<string> replacements, IEnumerable<LiteralViewModel> literals)
            => literals
                .Where(l => replacements.Any(r => r == l.Id) || l.Touched)
                .Concat(replacements
                    .Where(r => !literals.Any(l => l.Id == r))
                    .Select(r => new LiteralViewModel(r)))
                .ToList();
    }
}