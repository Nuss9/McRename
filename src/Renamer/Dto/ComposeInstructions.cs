using System.Collections.Generic;

namespace Renamer.Dto
{
    public class ComposeInstructions
	{
		public ComposeMode Mode { get; set; }
		public ComposeMode2 Mode2 { get; set; }
		public ComposeAction Action { get; set; }
		public string CustomText { get; set; }
		public List<FileInformation> Files { get; set; }
        public int InsertPosition { get; set; }
        public string TextToReplace { get; set; }

        public ComposeInstructions(ComposeMode2 mode2, ComposeAction action, List<FileInformation> files)
		{
			Mode2 = mode2;
			Action = action;
			Files = files;
		}

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

		public ComposeInstructions(ComposeMode mode, string customText, string textToReplace, List<FileInformation> files)
		{
			Mode = mode;
			CustomText = customText;
			TextToReplace = textToReplace;
			Files = files;
		}

        public bool Equals(ComposeInstructions instructions)
		{
			if(Mode != instructions.Mode) {
				return false;
			}

			if(CustomText != instructions.CustomText) {
				return false;
			}

			if(Files.Count != instructions.Files.Count) {
				return false;
			}

			for(int i = 0; i < Files.Count; i++) {
				if(Files[i].Path != instructions.Files[i].Path || 
					Files[i].CreationDateTime != instructions.Files[i].CreationDateTime) {
						return false;
					}
			}

			return true;
		}
	}
}
