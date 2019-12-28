using VisualStudioSnippetGenerator.Services;
using VisualStudioSnippetGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Pages
{
    public partial class Index
    {
        public Index()
        {
            Snippet = new VisualStudioSnippet();

            Snippet.CodeSnippet.Header.OnChanged += OnSnippetChanged;
            Snippet.CodeSnippet.Snippet.Declarations.OnChanged += OnSnippetChanged;
            Snippet.CodeSnippet.Snippet.Imports.OnChanged += OnSnippetChanged;
            Snippet.CodeSnippet.Snippet.Code.OnChanged += OnSnippetChanged;
        }

        public VisualStudioSnippet Snippet { get; set; }

        public string SnippetText { get; set; } = string.Empty;

#nullable disable
        private SnippetSerializer _snippetSerializer;

        [Inject]
        public SnippetSerializer SnippetSerializer
        {
            get => _snippetSerializer;
            set
            {
                _snippetSerializer = value;
                TrySerializeSnippet();
            }
        }

        [Inject]
        public ReplacementService ReplacementService { get; set; }
#nullable enable

        public string? Error { get; set; }

        public bool SyncEnabled { get; set; } = true;

        public bool IsCSharp => Snippet.CodeSnippet.Snippet.Code.Language
            .Equals(Constants.Languages.CSharp, StringComparison.OrdinalIgnoreCase);

        public void OnSnippetChanged(ObservableObjectChangedArgs e)
        {
            if (e.Sender is Code code && e.PropertyName == nameof(Code.Body) && SyncEnabled)
            {
                var replacements = ReplacementService.MatchReplacements(code.Body);
                Snippet.CodeSnippet.Snippet.Declarations.ReplaceTroughBackdoor(MapCodeToDeclarations(replacements));
                Snippet.CodeSnippet.Header.IsSurroundsWithBackdoor = replacements.Contains(Constants.ReservedKeywords.Selected);
            }
            else if (e.Sender is Declaration && e.PropertyName == nameof(Declaration.Identifier) && SyncEnabled)
            {
                Snippet.CodeSnippet.Snippet.Code.BodyBackdoor = ReplacementService.UpdateReplacements(
                    Snippet.CodeSnippet.Snippet.Code.Body, (string)e.PreviousValue!, (string)e.CurrentValue!);
            }

            TrySerializeSnippet();
            StateHasChanged();
        }

        public void TrySerializeSnippet()
        {
            try
            {
                Error = null;
                SnippetText = SnippetSerializer.Serialize(Snippet);
            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }
        }

        public void AddDeclaration()
        {
            var declaration = new Declaration(touched: true, focus: true);
            declaration.OnChanged += OnSnippetChanged;
            Snippet.CodeSnippet.Snippet.Declarations.Add(declaration);
        }

        public IEnumerable<Declaration> MapCodeToDeclarations(IEnumerable<string> replacements)
        {
            var afterMaping = Snippet.CodeSnippet.Snippet.Declarations
                .Where(d => replacements.Contains(d.Identifier) || d.Touched)
                .Concat(replacements
                    .Where(r => !Constants.ReservedKeywords.All.Contains(r)
                        && !Snippet.CodeSnippet.Snippet.Declarations.Any(d => d.Identifier == r))
                    .Select(r =>
                    {
                        var declaration = new Declaration(r);
                        declaration.OnChanged += OnSnippetChanged;
                        return declaration;
                    }));

            var removed = Snippet.CodeSnippet.Snippet.Declarations
                .Except(afterMaping);

            foreach (var toUnsubscribe in removed)
            {
                toUnsubscribe.OnChanged -= OnSnippetChanged;
            }

            return afterMaping;
        }

        public void RemoveDeclaration(Declaration declaration)
        {
            Snippet.CodeSnippet.Snippet.Declarations.Remove(declaration);
            declaration.OnChanged -= OnSnippetChanged;
        }

        public void AddImport()
        {
            var import = new Import();
            import.OnChanged += OnSnippetChanged;
            Snippet.CodeSnippet.Snippet.Imports.Add(import);
        }

        public void RemoveImportIfEmpty(Import import)
        {
            if (string.IsNullOrWhiteSpace(import.Namespace))
            {
                Snippet.CodeSnippet.Snippet.Imports.Remove(import);
                import.OnChanged -= OnSnippetChanged;
            }
        }

        public void MoveDeclarationUp(int index)
            => Snippet.CodeSnippet.Snippet.Declarations.Move(index, index - 1);

        public void MoveDeclarationDown(int index)
            => Snippet.CodeSnippet.Snippet.Declarations.Move(index, index + 1);
    }
}