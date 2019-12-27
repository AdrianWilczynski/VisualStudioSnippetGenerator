namespace VisualStudioSnippetGenerator.Models
{
    public class Import : UIIdentifiableBase
    {
        public Import() { }

        public Import(string @namespace)
        {
            Namespace = @namespace;
        }

        public string? Namespace { get; set; }
    }
}