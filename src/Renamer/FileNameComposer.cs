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
                case ComposeMode.Date:
                case ComposeMode.DateTime:
                case ComposeMode.Extension:
                case ComposeMode.Truncation:
                    var composer = ComposerFactory.Build(Instructions.Mode);
                    Proposal = composer.Rename(Instructions);
                    break;
                default:
                    throw new Exception("Should not reach this point.");
            }

            ControlDuplicateValues(Proposal);

            return Proposal;
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
