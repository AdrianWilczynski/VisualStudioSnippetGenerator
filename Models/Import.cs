using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Models
{
    public class Import : ObservableObject
    {
        private string _namespace = string.Empty;

        public string Namespace
        {
            get => _namespace;
            set => SetProperty(ref _namespace, value);
        }
    }
}