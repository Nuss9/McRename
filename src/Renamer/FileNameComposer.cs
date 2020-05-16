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
            RemoveHiddenFiles(ref instructions);

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
                case ComposeMode.CustomText:
                    var composer = ComposerFactory.Build(Instructions.Mode);
                    Proposal = composer.Rename(Instructions);
                    break;
                case ComposeMode.Truncation:
                    ComposeWithTruncation();
                    break;
                case ComposeMode.Extension:
                    ComposeExtensions();
                    break;
                default:
                    ComposeByCreationDateTime();
                    break;
            }

            ControlDuplicateValues(Proposal);

            return Proposal;
        }

        private void ComposeExtensions()
        {
            foreach(var file in Instructions.Files)
            {
                string newExtension = Instructions.CustomText.Trim('.');

                string path = file.Path;
                string newPath = Path.GetDirectoryName(path)
                    + Separator
                    + Path.GetFileNameWithoutExtension(path)
                    + $".{newExtension}";

                Proposal.Add(path, newPath);
            }
        }

        private void RemoveHiddenFiles(ref ComposeInstructions instructions)
        {
            for (int i = 0; i < instructions.Files.Count; i++)
            {
                string filename = Path.GetFileName(instructions.Files[i].Path);

                if (filename.StartsWith("."))
                {
                    instructions.Files.RemoveAt(i);
                }
            }
        }

        private void ComposeWithTruncation()
        {
            string truncationText = Instructions.CustomText;

            if (!Instructions.Files.Any(x => x.Path.Contains(truncationText)))
            {
                Proposal = ErrorMessage("Custom text to truncate not found in any filename.");
                return;
            }

            foreach(var file in Instructions.Files)
            {
                string path = file.Path;
                string newPath = string.Empty;

                if(Path.GetFileName(path).Contains(truncationText))
                {
                    string directory = Path.GetDirectoryName(path);
                    var extension = Path.GetExtension(path);

                    int index = path.IndexOf(truncationText);
                    int count = truncationText.Length;
                    newPath = path.Remove(index, count);
                }
                else
                {
                    continue;
                }

                Proposal.Add(path, newPath);
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
