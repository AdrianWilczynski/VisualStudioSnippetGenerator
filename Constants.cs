using System.Collections.Generic;

namespace VisualStudioSnippetGenerator
{
    public static class Constants
    {
        public static IEnumerable<string> Languages => new[]
        {
            "CSharp",
            "TypeScript",
            "JavaScript",
            "VB",
            "XML",
            "HTML",
            "CSS"
        };
    }
}