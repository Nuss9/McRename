using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Dto;
using Renamer.Exceptions;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class DateTimeComposer : BaseComposer, ICompose
    {
        private ComposeInstructions instructions;
        private int duplicateCounter = 1;
        private TempFileInfo tempFile;

        public Dictionary<string, string> Compose(ComposeInstructions input)
        {
            instructions = input;

            foreach (var file in instructions.Files)
            {
                GetTempFileInfo(file);

                ComposeBaseName();

                if (Composition.Any())
                {
                    HandleDuplicates();
                }

                Composition.Add(tempFile.Path, GetNewPath());
            }

            return Composition;
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
                CreationDateTime = file.CreationDateTime.ToString(GetTimeFormat(instructions.Action))
            };
        }

        private void ComposeBaseName()
        {
            switch (instructions.Mode2)
            {
                case ComposeMode2.Replace:
                    tempFile.BaseName = tempFile.CreationDateTime;
                    break;
                case ComposeMode2.Prepend:
                    tempFile.BaseName = tempFile.CreationDateTime + tempFile.BaseName;
                    break;
                case ComposeMode2.Insert:
                    try
                    {
                        tempFile.BaseName = InsertAtSpecifiedIndex();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        tempFile.BaseName = InsertAtBaseNameEnd();

                    }
                    break;
                case ComposeMode2.Append:
                    tempFile.BaseName += tempFile.CreationDateTime;
                    break;
                case ComposeMode2.Unknown:
                default:
                    throw new UnknownComposeModeException("Invalid mode.");
            }
        }

        private string InsertAtBaseNameEnd() => tempFile.BaseName.Insert(tempFile.BaseName.Count(), tempFile.CreationDateTime);

        private string InsertAtSpecifiedIndex() => tempFile.BaseName.Insert(instructions.InsertPosition, tempFile.CreationDateTime);

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

        private string GetNewPath() => $"{tempFile.Directory}{Separator}{tempFile.BaseName}{tempFile.Extension}";

        private void AddDuplicateNumberToCurrentEntry() => tempFile.BaseName += $"_({duplicateCounter})";

        private void AddDuplicateNumberToLastEntry()
        {
            string lastKey = Composition.Keys.Last();
            Composition.Remove(lastKey);
            Composition.Add(lastKey, $"{tempFile.Directory}{Separator}{tempFile.BaseName}_({duplicateCounter}){tempFile.Extension}");
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

        private void ResetDuplicateCounter() => duplicateCounter = 1;
    }
}
