using System.Collections.Generic;
using System.IO;

namespace Renamer.Composers
{
    public abstract class BaseComposer
    {
        public BaseComposer()
        {
            Composition = new Dictionary<string, string>();
            Separator = Path.DirectorySeparatorChar;
        }

        public Dictionary<string, string> Composition { get; private set; }
        public char Separator { get; }
    }
}
