using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class DateTimeComposer : BaseComposer, ICompose
    {
        private int duplicateCounter = 1;

        public Dictionary<string, string> Compose(ComposeInstructions instructions)
        {
            foreach (var file in instructions.Files)
            {
                var path = file.Path;
                var fileName = Path.GetFileName(path);
                var directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);
                var baseName = fileName.TrimEnd(extension.ToCharArray());

                string format = GetTimeFormat(instructions.Mode);
                var creationDate = file.CreationDateTime.ToString(format);

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
                        ResetDuplicateCounter();
                    }
                }

                string newPath = $"{directory}{Separator}{creationDate}{extension}";

                Composition.Add(path, newPath);
            }

            return Composition;
        }

        private string GetTimeFormat(ComposeMode mode)
        {
            return mode switch
            {
                ComposeMode.Date => "yyyyMMdd",
                ComposeMode.DateTime => "yyyyMMdd_HHmmss",
                _ => throw new ShouldNotOccurException("Invalid time format."),
            };
        }

        private void ResetDuplicateCounter()
        {
            duplicateCounter = 1;
        }
    }
}
