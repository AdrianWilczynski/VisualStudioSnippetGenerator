using System.Xml;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    public class SnippetType
    {
        public SnippetType() { }

        public SnippetType(string? code)
        {
            Body = code;
        }

        [XmlText]
        public string? Body { get; set; }
    }
}