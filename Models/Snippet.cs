using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    [XmlRoot("CodeSnippets")]
    public class VisualStudioSnippet
    {
        public VisualStudioSnippet() { }

        public VisualStudioSnippet(string title, string shortcut, string language, bool isExpansion, bool isSurroundsWith,
            List<Literal> literals, string body, string? description, string? author)
        {
            CodeSnippet = new CodeSnippet(
                new Header(title, shortcut, isExpansion, isSurroundsWith, description, author),
                new Snippet(
                    new Code(language, body), literals));
        }

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
        private string? _description;
        private string? _author;

        public Header() { }

        public Header(string title, string shortcut, bool isExpansion, bool isSurroundsWith, string? description, string? author)
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
        public string? Shortcut { get; set; }

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

        public List<SnippetType> SnippetTypes { get; set; } = new List<SnippetType>();
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

        public Snippet(Code code, List<Literal>? literals)
        {
            Code = code;
            Declarations = literals;
        }

        public bool DeclarationsSpecified => Declarations?.Count > 0;

        public List<Literal>? Declarations { get; set; }

        public Code? Code { get; set; }
    }

    public class Literal
    {
        private string _identifier = string.Empty;
        private string _defaultValue = string.Empty;
        private string? _toolTip;
        private string? _function;

        public Literal() { }

        public Literal(string identifier)
        {
            _identifier = identifier;
        }

        [XmlIgnore]
        public string UIIdentifier { get; } = Guid.NewGuid().ToString();

        [XmlElement("ID")]
        public string Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                Touched = true;
            }
        }

        [XmlElement("Default")]
        public string DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                Touched = true;
            }
        }

        public string? ToolTip
        {
            get => !string.IsNullOrWhiteSpace(_toolTip) ? _toolTip : null;
            set
            {
                _toolTip = value;
                Touched = true;
            }
        }

        public string? Function
        {
            get => !string.IsNullOrWhiteSpace(_function) ? _function : null;
            set
            {
                _function = value;
                Touched = true;
            }
        }

        [XmlIgnore]
        public bool Touched { get; set; }
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