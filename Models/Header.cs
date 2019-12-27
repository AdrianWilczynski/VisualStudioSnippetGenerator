using System.Collections.Generic;

namespace VisualStudioSnippetGenerator.Models
{
    public class Header
    {
        private string? _description;
        private string? _author;
        private string? _shortcut;

        public Header() { }

        public Header(string title, string? shortcut, bool isExpansion, bool isSurroundsWith, string? description, string? author)
        {
            Title = title;
            Shortcut = shortcut;
            Description = description;
            Author = author;

            if (isExpansion)
            {
                SnippetTypes.Add(new SnippetType(Constants.SnippetTypes.Expansion));
            }
            if (isSurroundsWith)
            {
                SnippetTypes.Add(new SnippetType(Constants.SnippetTypes.SurroundsWith));
            }
        }

        public string? Title { get; set; }

        public string? Shortcut
        {
            get => string.IsNullOrWhiteSpace(_shortcut) ? null : _shortcut;
            set => _shortcut = value;
        }

        public string? Description
        {
            get => string.IsNullOrWhiteSpace(_description) ? null : _description;
            set => _description = value;
        }

        public string? Author

        {
            get => string.IsNullOrWhiteSpace(_author) ? null : _author;
            set => _author = value;
        }

        public bool SnippetTypesSpecified => SnippetTypes.Count > 0;

        public List<SnippetType> SnippetTypes { get; set; } = new List<SnippetType>();
    }
}