using System.Xml;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Models
{
    public class Declaration : ObservableObject
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
            set => SetProperty(ref _identifier, value, onChanged: () => Touched = true);
        }

        public bool EditableSpecified => !Editable;

        [XmlAttribute]
        public bool Editable
        {
            get => _editable;
            set => SetProperty(ref _editable, value, onChanged: () => Touched = true);
        }

        [XmlElement("Default")]
        public string DefaultValue
        {
            get => _defaultValue;
            set => SetProperty(ref _defaultValue, value, onChanged: () => Touched = true);
        }

        public bool ToolTipSpecified => !string.IsNullOrWhiteSpace(_toolTip);

        public string? ToolTip
        {
            get => _toolTip;
            set => SetProperty(ref _toolTip, value, onChanged: () => Touched = true);
        }

        public bool FunctionSpecified => !string.IsNullOrWhiteSpace(_function);

        public string? Function
        {
            get => _function;
            set => SetProperty(ref _function, value, onChanged: () => Touched = true);
        }

        public bool TypeSpecified => !string.IsNullOrWhiteSpace(_type);

        public string? Type
        {
            get => _type;
            set => SetProperty(ref _type, value, onChanged: () => Touched = true);
        }

        [XmlIgnore]
        public bool Touched { get; set; }

        [XmlIgnore]
        public bool Focus { get; set; }
    }
}