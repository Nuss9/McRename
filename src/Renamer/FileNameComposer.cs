using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer
{
	public class FileNameComposer : IRename
	{
		public Dictionary<string, string> Rename(ComposeInstructions instructions)
		{
			var proposal = new Dictionary<string, string>();

			if (instructions.Mode == ComposeMode.Unknown || instructions.Files.Count == 0)
			{
				return proposal;
			}

			instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

			char separator = Path.DirectorySeparatorChar;
			int duplicateCounter = 1;

			for (int i = 0; i < instructions.Files.Count; i++)
			{
				var path = instructions.Files[i].Path;
				var directory = Path.GetDirectoryName(path);
				var extension = Path.GetExtension(path);

				string newPath;

				if(instructions.Mode == ComposeMode.Numerical) {
					newPath = $"{directory}{separator}{i+1}{extension}";
				}
				else {
					string dateFormat;
					if(instructions.Mode == ComposeMode.Date) {
						dateFormat = "yyyyMMdd";
					} else {
						dateFormat = "yyyyMMdd_HHmmss";
					}
					var creationDate = instructions.Files[i].CreationDateTime.ToString(dateFormat);

					if(proposal.Any()) {
						if(proposal.Values.Last().EndsWith($"{creationDate}{extension}")) {
							string lastKey = proposal.Keys.Last();
							proposal.Remove(lastKey);
							proposal.Add(lastKey, $"{directory}{separator}{creationDate}_({duplicateCounter}){extension}");

							duplicateCounter++;
							creationDate += $"_({duplicateCounter})";
							duplicateCounter++;
						} else if(proposal.Values.Last().Contains(creationDate)) {
							creationDate += $"_({duplicateCounter})";
							duplicateCounter++;
						} else {
							duplicateCounter = 1;
						}
					}

					newPath = $"{directory}{separator}{creationDate}{extension}";
				}

				proposal.Add(path, newPath);
			}

            ControlDuplicateValues(proposal);

            return proposal;
        }

        private static void ControlDuplicateValues(Dictionary<string, string> proposal)
        {
            if (proposal.Values.Distinct().Count() != proposal.Values.Count())
            {
                proposal.Clear();
                proposal.Add("Error message", "Aborted renaming due to duplicates in end result.");
            }
        }
    }
}
