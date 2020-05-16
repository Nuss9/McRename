using System.Collections.Generic;
using System.IO;

namespace Renamer
{
    internal class TextComposer : ICompose
    {
        private readonly char Separator;

        public TextComposer()
        {
            Separator = Path.DirectorySeparatorChar;
        }

        public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            string customText = instructions.CustomText;
            string directory = Path.GetDirectoryName(instructions.Files[0].Path);
            var extension = Path.GetExtension(instructions.Files[0].Path);

            int counter = 1;
            var proposal = new Dictionary<string, string>();

            foreach (var file in instructions.Files)
            {
                string newPath = $"{directory}{Separator}{customText}_({counter++}){extension}";
                proposal.Add(file.Path, newPath);
            }

            return proposal;
        }
    }
}
