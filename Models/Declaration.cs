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
}