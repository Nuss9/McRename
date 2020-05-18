using System;
using Renamer.Composers;

namespace Renamer
{
    public static class ComposerFactory
    {
        public static ICompose Build(ComposeMode mode)
        {
            switch (mode)
            {
                case ComposeMode.Numerical:
                    return new NumericalComposer();
                case ComposeMode.CustomText:
                    return new TextComposer();
                case ComposeMode.DateTime:
                    return new DateTimeComposer();
                case ComposeMode.Extension:
                    return new ExtensionComposer();
                case ComposeMode.Truncation:
                    return new TruncationComposer();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
