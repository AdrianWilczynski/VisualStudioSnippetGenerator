using System.IO;
using System.Text;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Models;

namespace VisualStudioSnippetGenerator.Services
{
    public class SnippetSerializer
    {
        private const string DefaultNamespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

        private readonly XmlSerializer _xmlSerializer;

        public SnippetSerializer()
        {
            _xmlSerializer = new XmlSerializer(typeof(VisualStudioSnippet), DefaultNamespace);
        }

        public string Serialize(VisualStudioSnippet snippet)
        {
            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, DefaultNamespace);

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

            _xmlSerializer.Serialize(streamWriter, snippet, xmlNamespaces);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}