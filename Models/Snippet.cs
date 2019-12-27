using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Models
{
    public class Snippet
    {
        public bool ImportsSpecified => Imports.Count > 0;

        public ObservableCollection<Import> Imports { get; set; } = new ObservableCollection<Import>();

        [XmlIgnore]
        public ObservableCollection<Declaration> Declarations { get; set; } = new ObservableCollection<Declaration>();

        public bool DeclarationsArraySpecified => Declarations.Count > 0;

        [XmlArray(nameof(Declarations))]
        [XmlArrayItem(nameof(Literal), typeof(Literal))]
        [XmlArrayItem(nameof(Object), typeof(Object))]
        public List<Declaration> DeclarationsArray
        {
            get => Declarations
                .Select(d => d.TypeSpecified ? new Object(d) : (Declaration)new Literal(d))
                .ToList();
            set { }
        }

        public Code Code { get; set; } = new Code();
    }
}