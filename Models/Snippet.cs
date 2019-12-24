using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    [XmlRoot("CodeSnippets")]
    public class VisualStudioSnippet
    {
        public VisualStudioSnippet() { }

        public VisualStudioSnippet(string title, string shortcut, string language, string body, string? description = null,
            string? author = null, IEnumerable<Literal>? literals = null)
        {
            CodeSnippet = new CodeSnippet(
                new Header(title, shortcut, description, author),
                new Snippet(
                    new Code(language, body), literals));
        }

        [XmlIgnore]
        public string Xmlns { get; } = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

        public CodeSnippet? CodeSnippet { get; set; }
    }

    public class CodeSnippet
    {
        public CodeSnippet() { }

        public CodeSnippet(Header header, Snippet snippet)
        {
            Header = header;
            Snippet = snippet;
        }

        [XmlAttribute]
        public string Format { get; set; } = "1.0.0";

        public Header? Header { get; set; }

        public Snippet? Snippet { get; set; }
    }

    public class Header
    {
        public Header() { }

        public Header(string title, string shortcut, string? description, string? author)
        {
            Title = title;
            Shortcut = shortcut;
            Description = description;
            Author = author;
        }

        public string? Title { get; set; }
        public string? Shortcut { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }

        public SnippetType[] SnippetTypes { get; set; } = new[]
        {
            new SnippetType("Expansion")
        };
    }

    public class SnippetType
    {
        public SnippetType() { }

        public SnippetType(string? body)
        {
            Body = body;
        }

        [XmlText]
        public string? Body { get; set; }
    }

    public class Snippet
    {
        public Snippet() { }

        public Snippet(Code code, IEnumerable<Literal>? literals)
        {
            Code = code;
            Declarations = literals?.ToArray();
        }

        public bool DeclarationsSpecified => Declarations?.Length > 0;

        public Literal[]? Declarations { get; set; }

        public Code? Code { get; set; }
    }

    public class Literal
    {
        public Literal() { }

        public Literal(string id, string defaultValue)
        {
            Id = id;
            DefaultValue = defaultValue;
        }

        [XmlElement("ID")]
        public string? Id { get; set; }

        [XmlElement("Default")]
        public string? DefaultValue { get; set; }
    }

    public class Code
    {
        public Code() { }

        public Code(string language, string body)
        {
            Language = language;

            Body = new[] { new XmlDocument().CreateCDataSection(body) };
        }

        [XmlAttribute]
        public string? Language { get; set; }

        [XmlText]
        public XmlNode[]? Body { get; set; }
    }
}