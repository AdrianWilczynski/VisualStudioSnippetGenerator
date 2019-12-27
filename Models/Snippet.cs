using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
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
}