using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Renamer
{
	public class FileRenamer : IRename
	{
		public Dictionary<string, string> Execute(RenameInstructions instructions)
		{
			var proposal = new Dictionary<string, string>();

			if (instructions.Mode == RenameMode.Unknown || instructions.Files.Count == 0)
			{
				return proposal;
			}

			instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

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
