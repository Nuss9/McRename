using System.Collections.Generic;
using Renamer.Dto;

namespace Renamer.Interfaces
{
    public interface IRename
    {
	    Dictionary<string, string> Rename(ComposeInstructions instructions);
    }
}
