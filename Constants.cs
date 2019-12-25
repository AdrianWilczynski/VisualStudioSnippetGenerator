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
            "CPP",
            "VB",
            "XML",
            "HTML",
            "CSS",
            "SQL"
        };

        public static class ReservedKeywords
        {
            public const string End = "end";
            public const string Selected = "selected";

            public static IEnumerable<string> All => new[]
            {
                End,
                Selected
            };
        }

        public static class SnippetTypes
        {
            public const string Expansion = "Expansion";
            public const string SurroundsWith = "SurroundsWith";
        }
    }
}