using System.Collections.Generic;
using System.IO;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class ExtensionComposer : BaseComposer, ICompose
    {
        public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            foreach (var file in instructions.Files)
            {
                string newExtension = instructions.CustomText.Trim('.');

                string path = file.Path;
                string newPath = Path.GetDirectoryName(path)
                    + Separator
                    + Path.GetFileNameWithoutExtension(path)
                    + $".{newExtension}";

                Composition.Add(path, newPath);
            }

            return Composition;
        }
    }
}
