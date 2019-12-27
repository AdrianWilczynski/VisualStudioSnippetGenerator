using System.Xml;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    public class SnippetType
    {
        public SnippetType() { }

        public SnippetType(string type)
        {
            Body = type;
        }

        [XmlText]
        public string? Body { get; set; }
    }
}