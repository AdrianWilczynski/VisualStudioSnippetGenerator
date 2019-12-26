using System.Collections.Generic;
using System.Linq;
using VisualStudioSnippetGenerator.Services;
using VisualStudioSnippetGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace VisualStudioSnippetGenerator.Pages
{
    public partial class Index
    {
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
        public SnippetSerializer SnippetSerializer
        {
            get => _snippetSerializer;
            set => SetThenSync(value, ref _snippetSerializer);
        }

        [Inject]
        public ReplacementService ReplacementService { get; set; }

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

                var replacements = ReplacementService.MatchReplacements(value);

                SyncBodyWithLiterals(replacements);
                SyncBodyWithSnippetType(replacements);
                Sync();
            }
        }

        public List<Literal> Literals { get; set; } = new List<Literal>();

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
            get => _description;
            set => SetThenSync(value, ref _description);
        }

        public string? Author
        {
            get => _author;
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

        public bool IsCSharp => Language.Equals(Constants.Languages.CSharp, StringComparison.OrdinalIgnoreCase);

        public string SnippetText { get; set; } = string.Empty;

        public void SetLiteralIdentifier(Literal literal, string newIdentifier)
        {
            if (ReplacementService.IsReplacementIdentifier(newIdentifier))
            {
                SyncLiteralIdentifierWithBody(literal.Identifier, newIdentifier);
            }

            literal.Identifier = newIdentifier;

            Sync();
        }

        public void SetDefaultLiteralValue(Literal literal, string newValue)
            => WithSync(() => literal.DefaultValue = newValue);

        public void SetLiteralToolTip(Literal literal, string newValue)
            => WithSync(() => literal.ToolTip = newValue);

        public void SetLiteralFunction(Literal literal, string newValue)
            => WithSync(() => literal.Function = newValue);

        public void RemoveLiteral(Literal literal)
            => WithSync(() => Literals.Remove(literal));

        public void MoveLiteralUp(int index)
            => WithSync(() => Literals.Reverse(index - 1, 2));

        public void MoveLiteralDown(int index)
            => WithSync(() => Literals.Reverse(index, 2));

        public void AddLiteral()
            => WithSync(() => Literals.Add(new Literal()));

        public void CopyToClipboard()
            => JSRuntime.InvokeVoidAsync("copyToClipboard", SnippetTextTextarea);

        public void SyncBodyWithLiterals(IEnumerable<string> replacements)
        {
            if (!SyncEnabled)
            {
                return;
            }

            Literals = ReplacementService.MapReplacementsToLiterals(replacements, Literals)
                .ToList();
        }

        public void SyncBodyWithSnippetType(IEnumerable<string> replacements)
        {
            if (!SyncEnabled)
            {
                return;
            }

            IsSurroundsWith = replacements.Contains(Constants.ReservedKeywords.Selected);
        }

        public void SyncLiteralIdentifierWithBody(string oldIdentifier, string newIdentifier)
        {
            if (!SyncEnabled)
            {
                return;
            }

            _body = ReplacementService.UpdateReplacements(Body, oldIdentifier, newIdentifier);
        }

        public override void Sync()
        {
            try
            {
                Error = null;

                SnippetText = SnippetSerializer.Serialize(
                    new VisualStudioSnippet(Title, Shortcut, Language, IsExpansion, IsSurroundsWith,
                        Literals, Body, Description, Author));
            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }
        }
    }
}