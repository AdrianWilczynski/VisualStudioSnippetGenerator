using System.Xml;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Models
{
    public class Code : ObservableObject
    {
        private string _body = string.Empty;
        private string _language = string.Empty;
        private string? _kind;
        private char _delimiter = Constants.Delimeter.Default;

        [XmlAttribute]
        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        public bool KindSpecified => !string.IsNullOrWhiteSpace(Kind);

        [XmlAttribute]
        public string? Kind
        {
            get => _kind;
            set => SetProperty(ref _kind, value);
        }

        [XmlIgnore]
        public char Delimiter
        {
            get => _delimiter;
            set => SetProperty(ref _delimiter,
                value != '\0' && !string.IsNullOrWhiteSpace(value.ToString())
                ? value
                : Constants.Delimeter.Default);
        }

        public bool DelimiterStringSpecified => Delimiter != Constants.Delimeter.Default;

        [XmlAttribute(nameof(Delimiter))]
        public string DelimiterString
        {
            get => Delimiter.ToString();
            set { }
        }

        [XmlText]
        public XmlNode[] BodyCData
        {
            get => new[] { new XmlDocument().CreateCDataSection(
                _body + (AppendEndKeyword ? Delimiter + Constants.ReservedKeywords.End + Delimiter : string.Empty)) };
            set { }
        }

        [XmlIgnore]
        public bool AppendEndKeyword { get; set; } = true;

        [XmlIgnore]
        public string Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }

        [XmlIgnore]
        public string BodyBackdoor
        {
            set => _body = value;
        }
    }
}