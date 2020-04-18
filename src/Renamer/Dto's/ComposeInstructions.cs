using System.Collections.Generic;

namespace Renamer
{
    public class ComposeInstructions
	{
		public ComposeMode Mode { get; set; }
		public string CustomText { get; set; }
		public List<FileInformation> Files { get; set; }

		public ComposeInstructions(ComposeMode mode, List<FileInformation> files)
		{
			Mode = mode;
			Files = files;
		}

        public ComposeInstructions(ComposeMode mode, string customText, List<FileInformation> files)
        {
			Mode = mode;
			CustomText = customText;
			Files = files;
        }
	}
}
