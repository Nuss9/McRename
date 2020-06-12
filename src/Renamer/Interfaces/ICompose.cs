using System.Collections.Generic;

namespace Renamer.Interfaces
{
    public interface ICompose
    {
        Dictionary<string, string> Rename(ComposeInstructions instructions);
    }
}
