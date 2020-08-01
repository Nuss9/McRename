using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Dto;
using Renamer.Exceptions;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class TextComposer : BaseComposer, ICompose
    {
        private int counter = 1;
        private int duplicateCounter = 1;
        private string customText;
        private TempFileInfo tempFile;
        private ComposeInstructions instructions;

        public Dictionary<string, string> Compose(ComposeInstructions input)
        {
            instructions = input;
            customText = instructions.CustomText;

            foreach (var file in instructions.Files)
            {
                GetTempFileInfo(file);

                ComposeBaseName();

                if (Composition.Any())
                {
                    HandleDuplicates();
                }

                Composition.Add(file.Path, GetNewPath());
            }

            return Composition;
        }

        private void ComposeBaseName()
        {
            switch (instructions.Mode2)
            {
                case ComposeMode2.Replace:
                    tempFile.BaseName = customText + "_(" + counter++.ToString() + ")";
                    break;
                case ComposeMode2.Unknown:
                default:
                    throw new UnknownComposeModeException("Invalid mode.");
            }
        }

        private void GetTempFileInfo(FileInformation file)
        {
            string path = file.Path;
            string fileName = Path.GetFileName(path);
            string extension = Path.GetExtension(path);

            tempFile = new TempFileInfo
            {
                Path = file.Path,
                FileName = Path.GetFileName(file.Path),
                BaseName = fileName.TrimEnd(extension.ToCharArray()),
                Directory = Path.GetDirectoryName(file.Path),
                Extension = extension,
                CreationDateTime = file.CreationDateTime.ToString(),
            };
        }

        private void HandleDuplicates()
        {
            if (DuplicateExistsWithoutNumber())
            {
                AddDuplicateNumberToLastEntry();

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
        }


        private bool DuplicateExistsWithNumber() => Composition.Values.Last().Contains(tempFile.BaseName);

        private bool DuplicateExistsWithoutNumber() => Composition.Values.Last().EndsWith($"{tempFile.BaseName}{tempFile.Extension}");

        private void AddDuplicateNumberToCurrentEntry() => tempFile.BaseName += $"_({duplicateCounter})";

        private void AddDuplicateNumberToLastEntry()
        {
            string lastKey = Composition.Keys.Last();
            Composition.Remove(lastKey);
            Composition.Add(lastKey, $"{tempFile.Directory}{Separator}{tempFile.BaseName}_({duplicateCounter}){tempFile.Extension}");
        }

        private void ResetDuplicateCounter() => duplicateCounter = 1;

        private string GetNewPath() => $"{tempFile.Directory}{Separator}{tempFile.BaseName}{tempFile.Extension}";
    }
}
