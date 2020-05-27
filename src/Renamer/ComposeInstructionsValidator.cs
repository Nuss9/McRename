using System.IO;

namespace Renamer
{
    public class ComposeInstructionsValidator
    {
        public void RemoveHiddenFiles(ref ComposeInstructions instructions)
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
        
        public string ValidateMode(ComposeMode mode)
        {
            if(mode == ComposeMode.Unknown)
            {
                return "Compose mode unknown.";
            }

            return string.Empty;
        }
    }
}
