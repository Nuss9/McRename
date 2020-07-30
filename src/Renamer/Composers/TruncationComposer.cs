using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Dto;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class TruncationComposer : BaseComposer, ICompose
    {
        public Dictionary<string, string> Compose(ComposeInstructions instructions)
        {
            string truncationText = instructions.CustomText;

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

                Composition.Add(path, newPath);
            }

            return Composition;
        }
    }
}
