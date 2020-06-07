using System.Collections.Generic;

namespace Renamer.Interfaces
{
    public interface IOrchestrate
    {
        Dictionary<string, string> Orchestrate(ComposeInstructions instructions);
    }
}
