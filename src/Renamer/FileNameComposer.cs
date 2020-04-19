using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer
{
    public class FileNameComposer : IRename
	{
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
                case ComposeMode.CustomText:
                    ComposeWithCustomText();
                    break;
                case ComposeMode.Truncation:
                    ComposeWithTruncation();
                    break;
                default:
                    ComposeByCreationDateTime();
                    break;
            }

            ControlDuplicateValues(Proposal);

            return Proposal;
        }

        private void ComposeWithTruncation()
        {
            if(!Instructions.Files.Any(x => x.Path.Contains(Instructions.CustomText)))
            {
                Proposal = ErrorMessage("Custom text to truncate not found in any filename.");
            }
        }

        private void ComposeWithCustomText()
        {
            string customText = Instructions.CustomText;
            string directory = Path.GetDirectoryName(Instructions.Files[0].Path);
            var extension = Path.GetExtension(Instructions.Files[0].Path);

            int counter = 1;

            foreach (var file in Instructions.Files)
            {
                string newPath = $"{directory}{Separator}{customText}_({counter++}){extension}";
                Proposal.Add(file.Path, newPath);
            }
        }

        private void ComposeByCreationDateTime()
        {
            int duplicateCounter = 1;

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
                        Proposal.Add(lastKey, $"{directory}{Separator}{creationDate}_({duplicateCounter}){extension}");

                        duplicateCounter++;
                        creationDate += $"_({duplicateCounter})";
                        duplicateCounter++;
                    }
                    else if (Proposal.Values.Last().Contains(creationDate))
                    {
                        creationDate += $"_({duplicateCounter})";
                        duplicateCounter++;
                    }
                    else
                    {
                        duplicateCounter = 1;
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
