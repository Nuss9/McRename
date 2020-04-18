using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer
{
    public class FileNameComposer : IRename
	{
        private int DuplicateCounter = 1;
        private readonly char Separator = Path.DirectorySeparatorChar;
		private ComposeInstructions Instructions;
		private Dictionary<string, string> Proposal = new Dictionary<string, string>();

		public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            if (instructions.Mode == ComposeMode.Unknown)
            {
                return ErrorMessage("Compose mode unknown.");
            }
            else if (instructions.Files.Count == 0)
            {
                return ErrorMessage("No files found in selected directory.");
            }
            else
            {
                Instructions = instructions;
            }

            Instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

            switch (Instructions.Mode)
            {
                case ComposeMode.Numerical:
                    ComposeNumerically();
                    break;
                default:
                    ComposeByDate();
                    break;
            }

            ControlDuplicateValues(Proposal);

            return Proposal;
        }

        private void ComposeByDate()
        {
            foreach (var file in Instructions.Files)
            {
                var path = file.Path;
                var directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);

                string dateFormat;
                if (Instructions.Mode == ComposeMode.Date)
                {
                    dateFormat = "yyyyMMdd";
                }
                else
                {
                    dateFormat = "yyyyMMdd_HHmmss";
                }
                var creationDate = file.CreationDateTime.ToString(dateFormat);

                if (Proposal.Any())
                {
                    if (Proposal.Values.Last().EndsWith($"{creationDate}{extension}"))
                    {
                        string lastKey = Proposal.Keys.Last();
                        Proposal.Remove(lastKey);
                        Proposal.Add(lastKey, $"{directory}{Separator}{creationDate}_({DuplicateCounter}){extension}");

                        DuplicateCounter++;
                        creationDate += $"_({DuplicateCounter})";
                        DuplicateCounter++;
                    }
                    else if (Proposal.Values.Last().Contains(creationDate))
                    {
                        creationDate += $"_({DuplicateCounter})";
                        DuplicateCounter++;
                    }
                    else
                    {
                        DuplicateCounter = 1;
                    }
                }

                string newPath = $"{directory}{Separator}{creationDate}{extension}";

                Proposal.Add(path, newPath);

            }
        }

        private void ComposeNumerically()
		{
			for (int i = 0; i < Instructions.Files.Count; i++)
			{
				var path = Instructions.Files[i].Path;
				var directory = Path.GetDirectoryName(path);
				var extension = Path.GetExtension(path);

				string newPath = $"{directory}{Separator}{i + 1}{extension}";

				Proposal.Add(path, newPath);
			}
		}

        private static void ControlDuplicateValues(Dictionary<string, string> Proposal)
        {
            if (Proposal.Values.Distinct().Count() != Proposal.Values.Count())
            {
                Proposal.Clear();
                Proposal.Add("Error message", "Aborted renaming due to duplicates in end result.");
            }
        }

		private Dictionary<string, string> ErrorMessage(string message)
		{
			return new Dictionary<string, string>()
			{
				{ "Error message", message }
			};
		}
    }
}
