using System.Collections.Generic;

namespace Renamer
{
	public class FileRenamer : IRename
	{
		public Dictionary<string, string> Execute(RenameInstructions instructions)
		{
			return new Dictionary<string, string>();
		}
	}
}