using System.Xml;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Models
{
    public class Code : ObservableObject
    {
        private string _body = string.Empty;
        private string _language = string.Empty;

        [XmlAttribute]
        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        [XmlText]
        public XmlNode[] BodyCData
        {
            get => new[] { new XmlDocument().CreateCDataSection(_body) };
            set { }
        }

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