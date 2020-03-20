using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer
{
	public class FileRenamer : IRename
	{
		public Dictionary<string, string> Execute(RenameInstructions instructions)
		{
			var proposal = new Dictionary<string, string>();

			if (instructions.Files.Count == 0)
			{
				return proposal;
			}

			char separator = Path.DirectorySeparatorChar;

			for (int i = 0; i < instructions.Files.Count; i++)
			{
				var path = instructions.Files[i].Path;
				var directory = Path.GetDirectoryName(path);
				var extension = Path.GetExtension(path);
				var newPath = $"{directory}{separator}{i+1}{extension}";

				proposal.Add(path, newPath);
			}

			return proposal;
		}
	}
}
