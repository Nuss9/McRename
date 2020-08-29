using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Dto;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class ReplaceComposer : BaseComposer, ICompose
    {
        private ComposeInstructions instructions;
        private TempFileInfo tempFile;
        private string textToReplace;
        private string customText;
        private int duplicateCounter = 1;

        public Dictionary<string, string> Compose(ComposeInstructions input)
        {
            instructions = input;
            textToReplace = instructions.TextToReplace;
            customText = instructions.CustomText;

            if (TextToReplaceIsNotFoundInAnyFileName())
            {
                return ErrorMessage("Custom text to replace not found in any filename.");
            }

            foreach (var file in instructions.Files)
            {
                GetTempFileInfo(file);

                if (tempFile.Path.Contains(textToReplace))
                {
                    ComposeBaseName();
                }

                if (Composition.Any())
                {
                    HandleDuplicates();
                }
            }

            return Composition;
        }

        private void ComposeBaseName()
        {
            switch (instructions.Mode2)
            {
                case ComposeMode2.Replace:
                case ComposeMode2.Unknown:
                default:
                    Composition.Add(tempFile.Path, tempFile.Path.Replace(textToReplace, customText));
                    //throw new UnknownComposeModeException("Invalid mode.");
                    break;
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

        private bool TextToReplaceIsNotFoundInAnyFileName() => !instructions.Files.Any(x => x.Path.Contains(textToReplace));

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
    }
}
