namespace VisualStudioSnippetGenerator.Utilities
{
    public class ObservableObjectChangedArgs
    {
        public ObservableObjectChangedArgs(object sender, object? currentValue = null,
            object? previousValue = null, string? propertyName = null)
        {
            Sender = sender;
            CurrentValue = currentValue;
            PreviousValue = previousValue;
            PropertyName = propertyName;
        }

        public object Sender { get; set; }
        public object? CurrentValue { get; set; }
        public object? PreviousValue { get; set; }
        public string? PropertyName { get; set; }
    }
}