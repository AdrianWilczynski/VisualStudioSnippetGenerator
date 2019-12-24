using System.IO;
using System.Text;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Models;

namespace VisualStudioSnippetGenerator.Services
{
    public class SnippetSerializer
    {
        public string Serialize(VisualStudioSnippet snippet)
        {
            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, snippet.Xmlns);

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

            new XmlSerializer(typeof(VisualStudioSnippet), snippet.Xmlns).Serialize(streamWriter, snippet, xmlNamespaces);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}