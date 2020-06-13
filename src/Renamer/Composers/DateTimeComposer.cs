using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class DateTimeComposer : BaseComposer, ICompose
    {
        public Dictionary<string, string> Compose(ComposeInstructions instructions)
        {
            int duplicateCounter = 1;

            foreach (var file in instructions.Files)
            {
                var path = file.Path;
                var directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);

                string dateFormat;
                if (instructions.Mode == ComposeMode.Date)
                {
                    dateFormat = "yyyyMMdd";
                }
                else
                {
                    dateFormat = "yyyyMMdd_HHmmss";
                }
                var creationDate = file.CreationDateTime.ToString(dateFormat);

                if (Composition.Any())
                {
                    if (Composition.Values.Last().EndsWith($"{creationDate}{extension}"))
                    {
                        string lastKey = Composition.Keys.Last();
                        Composition.Remove(lastKey);
                        Composition.Add(lastKey, $"{directory}{Separator}{creationDate}_({duplicateCounter}){extension}");

                        duplicateCounter++;
                        creationDate += $"_({duplicateCounter})";
                        duplicateCounter++;
                    }
                    else if (Composition.Values.Last().Contains(creationDate))
                    {
                        creationDate += $"_({duplicateCounter})";
                        duplicateCounter++;
                    }
                    else
                    {
                        duplicateCounter = 1;
                    }
                }

                string newPath = $"{directory}{Separator}{creationDate}{extension}";

                Composition.Add(path, newPath);
            }

            return Composition;
        }
    }
}
