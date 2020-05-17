using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer
{
    public class TruncationComposer : ICompose
    {
        private readonly char Separator;

        public TruncationComposer()
        {
            Separator = Path.DirectorySeparatorChar;
        }

        public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            string truncationText = instructions.CustomText;
            var proposal = new Dictionary<string, string>();

            if (!instructions.Files.Any(x => x.Path.Contains(truncationText)))
            {
                return ErrorMessage("Custom text to truncate not found in any filename.");
            }

            foreach (var file in instructions.Files)
            {
                string path = file.Path;
                string newPath = string.Empty;

                if (Path.GetFileName(path).Contains(truncationText))
                {
                    string directory = Path.GetDirectoryName(path);
                    var extension = Path.GetExtension(path);

                    int index = path.IndexOf(truncationText);
                    int count = truncationText.Length;
                    newPath = path.Remove(index, count);
                }
                else
                {
                    continue;
                }

                proposal.Add(path, newPath);
            }

            return proposal;
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

