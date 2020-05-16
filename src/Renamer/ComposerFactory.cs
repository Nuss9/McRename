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
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
