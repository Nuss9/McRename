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

            return Proposal;
        }
    }
}
