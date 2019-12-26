using System;
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

    public class Snippet
    {
        private List<Declaration> _declarations = new List<Declaration>();

        public Snippet() { }

        public Snippet(Code code, List<Import> imports, List<Declaration> literals)
        {
            Code = code;
            Declarations = literals;
            Imports = imports;
        }

        public bool ImportsSpecified => Imports.Count > 0;

        public List<Import> Imports { get; set; } = new List<Import>();

        public bool DeclarationsSpecified => _declarations.Count > 0;

        [XmlArray]
        [XmlArrayItem(nameof(Literal), typeof(Literal))]
        [XmlArrayItem(nameof(Object), typeof(Object))]
        public List<Declaration> Declarations
        {
            get => _declarations.Select(d => d.Type switch
            {
                null => new Literal(d),
                _ => (Declaration)new Object(d)
            }).ToList();
            set => _declarations = value;
        }

        public Code? Code { get; set; }
    }

    public class Import
    {
        [XmlIgnore]
        public string UIIdentifier { get; } = Guid.NewGuid().ToString();

        public Import() { }

        public Import(string @namespace)
        {
            Namespace = @namespace;
        }

        public string? Namespace { get; set; }
    }

    public class Declaration
    {
        private string _identifier = string.Empty;
        private bool _editable = true;
        private string _defaultValue = string.Empty;
        private string? _toolTip;
        private string? _function;
        private string? _type;

        public Declaration() { }

        public Declaration(bool touched, bool focus)
        {
            Touched = touched;
            Focus = focus;
        }

        public Declaration(string identifier)
        {
            _identifier = identifier;
        }

        public Declaration(Declaration declaration) : this(declaration.Identifier)
        {
            _defaultValue = declaration.DefaultValue;
            _editable = declaration.Editable;
            _toolTip = declaration.ToolTip;
            _function = declaration.Function;
            _type = declaration.Type;
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

        public bool EditableSpecified => !Editable;

        [XmlAttribute]
        public bool Editable
        {
            get => _editable;
            set
            {
                _editable = value;
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

        public string? Type
        {
            get => !string.IsNullOrWhiteSpace(_type) ? _type : null;
            set
            {
                _type = value;
                Touched = true;
            }
        }

        [XmlIgnore]
        public bool Touched { get; set; }

        [XmlIgnore]
        public bool Focus { get; set; }
    }

    public class Literal : Declaration
    {
        public Literal() { }

        public Literal(Declaration declaration) : base(declaration) { }
    }

    public class Object : Declaration
    {
        public Object() { }

        public Object(Declaration declaration) : base(declaration) { }
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