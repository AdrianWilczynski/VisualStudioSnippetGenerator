using System.Collections.Generic;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    [XmlRoot("CodeSnippets")]
    public class VisualStudioSnippet
    {
        public VisualStudioSnippet() { }

        public VisualStudioSnippet(string title, string? shortcut, string language, bool isExpansion, bool isSurroundsWith,
            List<Import> imports, List<Declaration> literals, string code, string? description, string? author)
        {
            CodeSnippet = new CodeSnippet(
                new Header(title, shortcut, isExpansion, isSurroundsWith, description, author),
                new Snippet(
                    new Code(language, code), imports, literals));
        }

        public CodeSnippet? CodeSnippet { get; set; }
    }
}