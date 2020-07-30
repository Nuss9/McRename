using System.Collections.Generic;
using System.IO;
using Renamer.Dto;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class TextComposer : BaseComposer, ICompose
    {
        public Dictionary<string, string> Compose(ComposeInstructions instructions)
        {
            string customText = instructions.CustomText;
            string directory = Path.GetDirectoryName(instructions.Files[0].Path);

            int counter = 1;

            foreach (var file in instructions.Files)
            {
                string extension = Path.GetExtension(file.Path);
                string newPath = $"{directory}{Separator}{customText}_({counter++}){extension}";
                Composition.Add(file.Path, newPath);
            }

            return Composition;
        }
    }
}
