using System.Collections.Generic;

namespace VisualStudioSnippetGenerator.Models
{
    public class Header
    {
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

        public bool ShortcutSpecified => !string.IsNullOrWhiteSpace(Shortcut);

        public string? Shortcut { get; set; }

        public bool DescriptionSpecified => !string.IsNullOrWhiteSpace(Description);

        public string? Description { get; set; }

        public bool AuthorSpecified => !string.IsNullOrWhiteSpace(Author);

        public string? Author { get; set; }

        public bool SnippetTypesSpecified => SnippetTypes.Count > 0;

        public List<SnippetType> SnippetTypes { get; set; } = new List<SnippetType>();
    }
}