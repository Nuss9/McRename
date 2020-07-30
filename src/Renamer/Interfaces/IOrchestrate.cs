using System.Collections.Generic;
using Renamer.Dto;

namespace Renamer.Interfaces
{
    public interface IOrchestrate
    {
        Dictionary<string, string> Orchestrate(ComposeInstructions instructions);
    }
}
