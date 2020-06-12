using System.Collections.Generic;
using System.IO;
using Renamer.Interfaces;

namespace Renamer
{
    public class ComposeInstructionsValidator : IValidateComposeInstructions
    {
        public (bool isValid, string errorMessage) Validate(ref ComposeInstructions instructions)
        {
            RemoveHiddenFiles(ref instructions);

            string errorMessage = ValidateMode(instructions.Mode);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                return (false, errorMessage);
            }

            errorMessage = ValidateFilesCount(instructions.Files);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                return (false, errorMessage);
            }

            return (true, string.Empty);
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

        private string ValidateMode(ComposeMode mode)
        {
            if(mode == ComposeMode.Unknown)
            {
                return "Compose mode unknown.";
            }

            return string.Empty;
        }

        private string ValidateFilesCount(List<FileInformation> files)
        {
            if(files.Count == 0)
            {
                return "No files found in selected directory.";
            }

            return string.Empty;
        }
    }
}
