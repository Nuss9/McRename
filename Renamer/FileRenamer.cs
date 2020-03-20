using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

			string path = instructions.Files.FirstOrDefault().Path;
			char s = Path.DirectorySeparatorChar;
			string pathToDirectory = Path.GetDirectoryName(path);
			string extension = Path.GetExtension(path);
			string newPath = $"{pathToDirectory}{s}1{extension}";

			proposal.Add(path, newPath);
			
			return proposal;
		}
	}
}