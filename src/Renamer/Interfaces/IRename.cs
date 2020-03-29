using System.Collections.Generic;

namespace Renamer
{
    public interface IRename
    {
	    Dictionary<string, string> Rename(RenameInstructions instructions);
    }
}
