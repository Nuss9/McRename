using System.Collections;
using System.Collections.Generic;

namespace Renamer
{
    public class RenameInstructions
	{
		public RenameMode Mode { get; set; }
		public List<FileInformation> Files { get; set; }

		public RenameInstructions(RenameMode mode, List<FileInformation> files)
		{
			Mode = mode;
			Files = files;
		}
	}
}
