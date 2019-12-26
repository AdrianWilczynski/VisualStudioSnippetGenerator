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


#nullable disable
        private SnippetSerializer _snippetSerializer;

        [Inject]
        private SnippetSerializer SnippetSerializer
        {
            get => _snippetSerializer;
            set => SetThenSync(value, ref _snippetSerializer);
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
            set => SetThenSync(value, ref _title);
        }

        public string Shortcut
        {
            get => _shortcut;
            set => SetThenSync(value, ref _shortcut);
        }

        public string Language
        {
            get => _language;
            set => SetThenSync(value, ref _language);
        }

        public string? Description
        {
            get => string.IsNullOrWhiteSpace(_description) ? null : _description;
            set => SetThenSync(value, ref _description);
        }

        public string? Author
        {
            get => string.IsNullOrWhiteSpace(_author) ? null : _author;
            set => SetThenSync(value, ref _author);
        }

        public bool IsExpansion
        {
            get => _isExpansion;
            set => SetThenSync(value, ref _isExpansion);
        }

        public bool IsSurroundsWith
        {
            get => _isSurroundsWith;
            set => SetThenSync(value, ref _isSurroundsWith);
        }

        public string SnippetText { get; set; } = string.Empty;

        public void SetLiteralId(LiteralViewModel literal, string newValue)
        {
            if (IsReplacement(AsReplacement(newValue)))
            {
                SyncLiteralIdWithBody(literal.Id, newValue);
            }

            literal.Id = newValue;

            SyncWithSnippetText();
        }

        public void SetDefaultLiteralValue(LiteralViewModel literal, string newValue)
            => WithSync(() => literal.DefaultValue = newValue);

        public void RemoveLiteral(LiteralViewModel literal)
            => WithSync(() => Literals.Remove(literal));

        public void AddLiteral()
            => WithSync(() => Literals.Add(new LiteralViewModel()));

        public void CopyToClipboard()
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

        private void SyncWithSnippetText()
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

        private void SetThenSync<T>(T value, ref T backingField)
        {
            backingField = value;
            SyncWithSnippetText();
        }

        private void WithSync(Action action)
        {
            action();
            SyncWithSnippetText();
        }

        private static string AsReplacement(string id)
            => $"${id}$";

        private static bool IsReplacement(string replacement)
            => Regex.IsMatch(replacement, $"^{ReplacementRegex}$");

        private static IEnumerable<string> MatchReplacements(string body)
            => Regex
                .Matches(body, ReplacementRegex)
                .Select(m => m.Groups[1].Value);

        private static List<LiteralViewModel> MapReplacementsToLiterals(
            IEnumerable<string> replacements, IEnumerable<LiteralViewModel> literals)
            => literals
                .Where(l => replacements.Any(r => r == l.Id) || l.Touched)
                .Concat(replacements
                    .Where(r => !Constants.ReservedKeywords.All.Contains(r) && !literals.Any(l => l.Id == r))
                    .Select(r => new LiteralViewModel(r)))
                .ToList();
    }
}