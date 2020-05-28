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
            Instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

            var composer = ComposerFactory.Build(Instructions.Mode);
            Proposal = composer.Rename(Instructions);

            ControlDuplicateValues(Proposal);

            return Proposal;
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
