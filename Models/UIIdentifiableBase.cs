using System;
using System.Xml.Serialization;

namespace VisualStudioSnippetGenerator.Models
{
    public abstract class UIIdentifiableBase
    {
        [XmlIgnore]
        public string UIIdentifier { get; } = Guid.NewGuid().ToString();
    }
}