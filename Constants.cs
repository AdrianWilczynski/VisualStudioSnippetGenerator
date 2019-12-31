using System.Collections.Generic;

namespace VisualStudioSnippetGenerator
{
    public static class Constants
    {
        public const string SnippetFileExtension = ".snippet";

        public const string DefaultSnippetName = "new_snippet";

        public static class Languages
        {
            public const string CSharp = "CSharp";

            public static IEnumerable<string> All => new[]
            {
                CSharp,
                "TypeScript",
                "JavaScript",
                "CPP",
                "VB",
                "XML",
                "HTML",
                "CSS",
                "SQL"
            };
        }

        public static IEnumerable<string> Kinds => new[]
        {
            "method body",
            "method decl",
            "type decl",
            "page",
            "file",
            "any"
        };

        public static class Delimeter
        {
            public const char Default = '$';
        }

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

        public static IEnumerable<string> Functions => new[]
        {
            "GenerateSwitchCases($expression$)",
            "ClassName()",
            "SimpleTypeName(global::System.Console)"
        };
    }
}