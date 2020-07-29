﻿using System.Collections.Generic;
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
                baseName = file.CreationDateTime.ToString(format);

                if (Composition.Any())
                {
                    baseName = HandleDuplicates(directory, extension, baseName);
                }

                string newPath = GetNewPath(directory, extension, baseName);

                Composition.Add(path, newPath);
            }

            return Composition;
        }

        private string HandleDuplicates(string directory, string extension, string baseName)
        {
            if (DuplicateExistsWithoutNumber(extension, baseName))
            {
                AddNumberToLastEntry(directory, extension, baseName);

                duplicateCounter++;
                baseName = AddNumberToCurrentEntry(baseName);
                duplicateCounter++;
            }
            else if (DuplicateExistsWithNumber(baseName))
            {
                baseName = AddNumberToCurrentEntry(baseName);
                duplicateCounter++;
            }
            else
            {
                ResetDuplicateCounter();
            }

            return baseName;
        }

        private bool DuplicateExistsWithNumber(string baseName)
        {
            return Composition.Values.Last().Contains(baseName);
        }

        private bool DuplicateExistsWithoutNumber(string extension, string baseName)
        {
            return Composition.Values.Last().EndsWith($"{baseName}{extension}");
        }

        private string GetNewPath(string directory, string extension, string baseName)
        {
            return $"{directory}{Separator}{baseName}{extension}";
        }

        private string AddNumberToCurrentEntry(string baseName)
        {
            baseName += $"_({duplicateCounter})";
            return baseName;
        }

        private void AddNumberToLastEntry(string directory, string extension, string baseName)
        {
            string lastKey = Composition.Keys.Last();
            Composition.Remove(lastKey);
            Composition.Add(lastKey, $"{directory}{Separator}{baseName}_({duplicateCounter}){extension}");
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
