using System;
using System.Collections.Generic;
using System.IO;
using Renamer.Dto;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class NumericalComposer : BaseComposer, ICompose
    {
        public Dictionary<string, string> Compose(ComposeInstructions instructions)
        {
            instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

            for (int i = 0; i < instructions.Files.Count; i++)
            {
                var path = instructions.Files[i].Path;
                var directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);

                string newPath = $"{directory}{Separator}{i + 1}{extension}";

                Composition.Add(path, newPath);
            }

            return Composition;
        }
    }
}
