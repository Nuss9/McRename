using System;

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
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
