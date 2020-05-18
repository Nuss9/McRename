using System.Collections.Generic;

namespace Renamer
{
    public interface ICompose
    {
        Dictionary<string, string> Rename(ComposeInstructions instructions);
    }
}
