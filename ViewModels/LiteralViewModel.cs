using System;

namespace VisualStudioSnippetGenerator.ViewModels
{
    public class LiteralViewModel
    {
        public string Key { get; } = Guid.NewGuid().ToString();

        private string _id;
        private string _defaultValue = string.Empty;
        private string _tooltip = string.Empty;

        public LiteralViewModel(string id = "")
        {
            _id = id;
        }

        public string Id
        {
            get => _id;
            set
            {
                Touched = true;
                _id = value;
            }
        }

        public string DefaultValue
        {
            get => _defaultValue;
            set
            {
                Touched = true;
                _defaultValue = value;
            }
        }

        public string Tooltip
        {
            get => _tooltip;
            set
            {
                Touched = true;
                _tooltip = value;
            }
        }

        public bool Touched { get; private set; }
    }
}