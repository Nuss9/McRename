using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer.Composers
{
    public class FileNameComposer : IRename
	{
		private ComposeInstructions Instructions;
		private Dictionary<string, string> Proposal = new Dictionary<string, string>();

		public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            RemoveHiddenFiles(ref instructions);

            if (instructions.Mode == ComposeMode.Unknown)
            {
                return ErrorMessage("Compose mode unknown.");
            }

            if (instructions.Files.Count == 0)
            {
                return ErrorMessage("No files found in selected directory.");
            }

            Instructions = instructions;

            Instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

            var composer = ComposerFactory.Build(Instructions.Mode);
            Proposal = composer.Rename(Instructions);

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
