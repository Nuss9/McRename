using System.Collections.Generic;
using Renamer.Dto;

namespace Renamer.Interfaces
{
    public interface ICompose
    {
        Dictionary<string, string> Compose(ComposeInstructions instructions);
    }
}
