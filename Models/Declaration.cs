using System.Xml;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    public class Declaration : UIIdentifiableBase
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

        public bool ToolTipSpecified => !string.IsNullOrWhiteSpace(_toolTip);

        public string? ToolTip
        {
            get => _toolTip;
            set
            {
                _toolTip = value;
                Touched = true;
            }
        }

        public bool FunctionSpecified => !string.IsNullOrWhiteSpace(_function);

        public string? Function
        {
            get => _function;
            set
            {
                _function = value;
                Touched = true;
            }
        }

        public bool TypeSpecified => !string.IsNullOrWhiteSpace(_type);

        public string? Type
        {
            get => _type;
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
}