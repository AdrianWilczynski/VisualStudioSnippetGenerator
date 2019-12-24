namespace VisualStudioSnippetGenerator.ViewModels
{
    public class ReplacementViewModel
    {
        public ReplacementViewModel(string id, int position)
        {
            Id = id;
            Position = position;
        }

        public string Id { get; }
        public int Position { get; }
    }
}