using System.Collections.Generic;
using System.IO;

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