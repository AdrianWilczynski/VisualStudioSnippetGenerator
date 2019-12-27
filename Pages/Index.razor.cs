using System.Collections.Generic;
using System.Linq;
using VisualStudioSnippetGenerator.Services;
using VisualStudioSnippetGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace VisualStudioSnippetGenerator.Pages
{
    public partial class Index
    {
        private string _code = string.Empty;
        private string? _description;
        private string? _author;
        private string _language = string.Empty;
        private string? _shortcut;
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

        public string Code
        {
            get => _code;
            set
            {
                _code = value;

                var replacements = ReplacementService.MatchReplacements(value);

                SyncCodeWithDeclarations(replacements);
                SyncCodeWithSnippetType(replacements);
                Sync();
            }
        }

        public List<Declaration> Declarations { get; set; } = new List<Declaration>();
        public List<Import> Imports { get; set; } = new List<Import>();

        public string Title
        {
            get => _title;
            set => SetThenSync(value, ref _title);
        }

        public string? Shortcut
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

        public void SetDeclarationIdentifier(Declaration declaration, string newIdentifier)
        {
            if (ReplacementService.IsIdentifier(newIdentifier))
            {
                SyncDeclarationIdentifierWithCode(declaration.Identifier, newIdentifier);
            }

            declaration.Identifier = newIdentifier;

            Sync();
        }

        public void SetDefaultDeclarationValue(Declaration declaration, string newValue)
            => WithSync(() => declaration.DefaultValue = newValue);

        public void SetDeclarationToolTip(Declaration declaration, string newValue)
            => WithSync(() => declaration.ToolTip = newValue);

        public void SetDeclarationType(Declaration declaration, string newValue)
            => WithSync(() => declaration.Type = newValue);

        public void SetDeclarationFunction(Declaration declaration, string newValue)
            => WithSync(() => declaration.Function = newValue);

        public void SetDeclarationEditable(Declaration declaration, bool newValue)
            => WithSync(() => declaration.Editable = newValue);

        public void MoveDeclarationUp(int index)
            => WithSync(() => Declarations.Reverse(index - 1, 2));

        public void MoveDeclarationDown(int index)
            => WithSync(() => Declarations.Reverse(index, 2));

        public void AddDeclaration()
            => WithSync(() => Declarations.Add(new Declaration(touched: true, focus: true)));

        public void SetImport(Import import, string newValue)
            => WithSync(() => import.Namespace = newValue);

        public void RemoveImportIfEmpty(Import import)
            => WithSync(() =>
            {
                if (string.IsNullOrWhiteSpace(import.Namespace))
                {
                    Imports.Remove(import);
                }
            });

        public void AddImport()
            => WithSync(() => Imports.Add(new Import()));

        public void RemoveDeclaration(Declaration declaration)
            => WithSync(() => Declarations.Remove(declaration));

        public async Task CopyToClipboardAsync()
            => await JSRuntime.InvokeVoidAsync("copyToClipboard", SnippetTextTextarea);

        public void SyncCodeWithDeclarations(IEnumerable<string> replacements)
        {
            if (SyncEnabled)
            {
                Declarations = ReplacementService
                    .MapReplacementsToDeclarations(replacements, Declarations)
                    .ToList();
            }
        }

        public void SyncCodeWithSnippetType(IEnumerable<string> replacements)
        {
            if (SyncEnabled)
            {
                IsSurroundsWith = replacements.Contains(Constants.ReservedKeywords.Selected);
            }
        }

        public void SyncDeclarationIdentifierWithCode(string oldIdentifier, string newIdentifier)
        {
            if (SyncEnabled)
            {
                _code = ReplacementService.UpdateReplacements(Code, oldIdentifier, newIdentifier);
            }
        }

        public override void Sync()
        {
            try
            {
                Error = null;

                SnippetText = SnippetSerializer.Serialize(
                    new VisualStudioSnippet(Title, Shortcut, Language, IsExpansion, IsSurroundsWith,
                        Imports, Declarations, Code, Description, Author));
            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }
        }
    }
}