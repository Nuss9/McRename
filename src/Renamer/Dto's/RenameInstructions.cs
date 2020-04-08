using System.Collections.Generic;

namespace Renamer
{
    public class RenameInstructions
	{
		public ComposeMode Mode { get; set; }
		public List<FileInformation> Files { get; set; }

		public RenameInstructions(ComposeMode mode, List<FileInformation> files)
		{
			Mode = mode;
			Files = files;
		}
	}
}
