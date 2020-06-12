using System;
using Renamer.Composers;
using Renamer.Interfaces;

namespace Renamer
{
    public class ComposerFactory : IBuildComposer
    {
        public ICompose Build(ComposeMode mode)
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
                case ComposeMode.Date:
                    return new DateTimeComposer();
                case ComposeMode.Unknown:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
