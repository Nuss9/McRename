using System.Collections.Generic;
using System.IO;

namespace Renamer
{
    public class ExtensionComposer : ICompose
    {
        private readonly char Separator;

        public ExtensionComposer()
        {
            Separator = Path.DirectorySeparatorChar;
        }

        public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            var proposal = new Dictionary<string, string>();

            foreach (var file in instructions.Files)
            {
                string newExtension = instructions.CustomText.Trim('.');

                string path = file.Path;
                string newPath = Path.GetDirectoryName(path)
                    + Separator
                    + Path.GetFileNameWithoutExtension(path)
                    + $".{newExtension}";

                proposal.Add(path, newPath);
            }

            return proposal;
        }
    }
}
