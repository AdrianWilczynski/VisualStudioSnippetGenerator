using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VisualStudioSnippetGenerator.ViewModels;
using VisualStudioSnippetGenerator.Services;
using VisualStudioSnippetGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

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
        private bool _isExpansion = true;
        private bool _isSurroundsWith;

        private SnippetSerializer? _snippetSerializer;

#nullable disable
        [Inject]
        private SnippetSerializer SnippetSerializer
        {
            get => _snippetSerializer;
            set
            {
                _snippetSerializer = value;

                SyncWithSnippetText();
            }
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
#nullable enable

        private ElementReference SnippetTextTextarea { get; set; }

        public string? Error { get; set; }

        public bool SyncEnabled { get; set; } = true;

        public string Body
        {
            get => _body;
            set
            {
                _body = value;

                var replacements = MatchReplacements(value);

                SyncBodyWithLiterals(replacements);
                SyncBodyWithSnippetType(replacements);
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

        public bool IsExpansion
        {
            get => _isExpansion;
            set
            {
                _isExpansion = value;

                SyncWithSnippetText();
            }
        }

        public bool IsSurroundsWith
        {
            get => _isSurroundsWith;
            set
            {
                _isSurroundsWith = value;

                SyncWithSnippetText();
            }
        }

        public string SnippetText { get; set; } = string.Empty;

        public void OnLiteralIdChange(LiteralViewModel literal, string newValue)
        {
            if (IsReplacement(AsReplacement(newValue)))
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

        public void OnAddLiteralClick()
        {
            Literals.Add(new LiteralViewModel());

            SyncWithSnippetText();
        }

        public void OnCopyToClipboardClick()
            => JSRuntime.InvokeVoidAsync("copyToClipboard", SnippetTextTextarea);

        public void SyncBodyWithLiterals(IEnumerable<string> replacements)
        {
            if (!SyncEnabled)
            {
                return;
            }

            Literals = MapReplacementsToLiterals(replacements, Literals);
        }

        public void SyncBodyWithSnippetType(IEnumerable<string> replacements)
        {
            if (!SyncEnabled)
            {
                return;
            }

            IsSurroundsWith = replacements.Contains(Constants.ReservedKeywords.Selected);
        }

        public void SyncLiteralIdWithBody(string oldValue, string newValue)
        {
            if (!SyncEnabled)
            {
                return;
            }

            _body = Regex.Replace(_body, $@"(?<!\$)\${Regex.Escape(oldValue)}\$(?!\$)", AsReplacement(newValue));
        }

        public void SyncWithSnippetText()
        {
            try
            {
                SnippetText = SnippetSerializer.Serialize(
                    new VisualStudioSnippet(Title, Shortcut, Language, IsExpansion, IsSurroundsWith, Body, Description, Author,
                        Literals.Select(l => new Literal(l.Id, l.DefaultValue))));
            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }
        }

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
                    .Where(r => !Constants.ReservedKeywords.All.Contains(r) && !literals.Any(l => l.Id == r))
                    .Select(r => new LiteralViewModel(r)))
                .ToList();
    }
}