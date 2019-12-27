using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    [XmlRoot("CodeSnippets")]
    public class VisualStudioSnippet
    {
        public CodeSnippet CodeSnippet { get; set; } = new CodeSnippet();
    }
}