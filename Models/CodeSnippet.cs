using System.Xml;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    public class CodeSnippet
    {
        [XmlAttribute]
        public string Format { get; set; } = "1.0.0";

        public Header Header { get; set; } = new Header();

        public Snippet Snippet { get; set; } = new Snippet();
    }
}