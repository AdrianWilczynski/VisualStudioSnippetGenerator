using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VisualStudioSnippetGenerator.Utilities;

namespace VisualStudioSnippetGenerator.Models
{
    public class Header : ObservableObject
    {
        private bool _isSurroundsWith;
        private bool _isExpansion = true;
        private string? _title = string.Empty;
        private string? _shortcut;
        private string? _description;
        private string? _author;

        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool ShortcutSpecified => !string.IsNullOrWhiteSpace(Shortcut);

        public string? Shortcut
        {
            get => _shortcut;
            set => SetProperty(ref _shortcut, value);
        }

        public bool DescriptionSpecified => !string.IsNullOrWhiteSpace(Description);

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool AuthorSpecified => !string.IsNullOrWhiteSpace(Author);

        public string? Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        [XmlIgnore]
        public bool IsExpansion
        {
            get => _isExpansion;
            set => SetProperty(ref _isExpansion, value);
        }

        [XmlIgnore]
        public bool IsSurroundsWith
        {
            get => _isSurroundsWith;
            set => SetProperty(ref _isSurroundsWith, value);
        }

        public bool SnippetTypesSpecified => SnippetTypes.Count > 0;

        public List<SnippetType> SnippetTypes
        {
            get => new List<SnippetType>()
                .Append(IsExpansion ? new SnippetType(Constants.SnippetTypes.Expansion) : null)
                .Append(IsSurroundsWith ? new SnippetType(Constants.SnippetTypes.SurroundsWith) : null)
                .Where(s => s != null)
                .ToList()!;
            set { }
        }
    }
}