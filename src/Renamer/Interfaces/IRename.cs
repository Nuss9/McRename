using System.Collections.Generic;

namespace Renamer.Interfaces
{
    public interface IRename
    {
	    Dictionary<string, string> Rename(ComposeInstructions instructions);
    }
}
