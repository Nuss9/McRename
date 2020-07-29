using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class DateTimeComposer : BaseComposer, ICompose
    {
        private ComposeInstructions instructions;
        private int duplicateCounter = 1;
        private string baseName;

        public Dictionary<string, string> Compose(ComposeInstructions input)
        {
            instructions = input;

            foreach (var file in instructions.Files)
            {
                var path = file.Path;
                var fileName = Path.GetFileName(path);
                var directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);
                baseName = fileName.TrimEnd(extension.ToCharArray());

                string format = GetTimeFormat(instructions.Action);
                string fileCreationDateTime = file.CreationDateTime.ToString(format);

                ComposeBaseName(fileCreationDateTime);

                if (Composition.Any())
                {
                    baseName = HandleDuplicates(directory, extension);
                }

                string newPath = GetNewPath(directory, extension);

                Composition.Add(path, newPath);
            }

            return Composition;
        }

        private void ComposeBaseName(string createdDateTime)
        {
            switch (instructions.Mode2)
            {
                case ComposeMode2.Replace:
                    baseName = createdDateTime;
                    break;
                case ComposeMode2.Prepend:
                    baseName = createdDateTime + baseName;
                    break;
            }
        }

        private string HandleDuplicates(string directory, string extension)
        {
            if (DuplicateExistsWithoutNumber(extension))
            {
                AddDuplicateNumberToLastEntry(directory, extension);

                duplicateCounter++;
                AddDuplicateNumberToCurrentEntry();
                duplicateCounter++;
            }
            else if (DuplicateExistsWithNumber())
            {
                AddDuplicateNumberToCurrentEntry();
                duplicateCounter++;
            }
            else
            {
                ResetDuplicateCounter();
            }

            return baseName;
        }

        private bool DuplicateExistsWithNumber()
        {
            return Composition.Values.Last().Contains(baseName);
        }

        private bool DuplicateExistsWithoutNumber(string extension)
        {
            return Composition.Values.Last().EndsWith($"{baseName}{extension}");
        }

        private string GetNewPath(string directory, string extension)
        {
            return $"{directory}{Separator}{baseName}{extension}";
        }

        private void AddDuplicateNumberToCurrentEntry()
        {
            baseName += $"_({duplicateCounter})";
        }

        private void AddDuplicateNumberToLastEntry(string directory, string extension)
        {
            string lastKey = Composition.Keys.Last();
            Composition.Remove(lastKey);
            Composition.Add(lastKey, $"{directory}{Separator}{baseName}_({duplicateCounter}){extension}");
        }

        private string GetTimeFormat(ComposeAction action)
        {
            return action switch
            {
                ComposeAction.Date => "yyyyMMdd",
                ComposeAction.DateTime => "yyyyMMdd_HHmmss",
                _ => throw new ShouldNotOccurException("Invalid time format."),
            };
        }

        private void ResetDuplicateCounter()
        {
            duplicateCounter = 1;
        }
    }
}
