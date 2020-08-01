using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renamer.Dto;
using Renamer.Exceptions;
using Renamer.Interfaces;

namespace Renamer.Composers
{
    public class NumericalComposer : BaseComposer, ICompose
    {
        private TempFileInfo tempFile;
        private ComposeInstructions instructions;
        private int counter = 1;

        public Dictionary<string, string> Compose(ComposeInstructions input)
        {
            instructions = input;
            instructions.Files.Sort((x, y) => DateTime.Compare(x.CreationDateTime, y.CreationDateTime));

            foreach(var file in instructions.Files)
            {
                GetTempFileInfo(file);

                ComposeBaseName();

                string newPath = $"{tempFile.Directory}{Separator}{tempFile.BaseName}{tempFile.Extension}";
                Composition.Add(tempFile.Path, newPath);

                counter++;
            }

            return Composition;
        }

        private void ComposeBaseName()
        {
            switch (instructions.Mode2)
            {
                case ComposeMode2.Replace:
                    tempFile.BaseName = counter.ToString();
                    break;
                case ComposeMode2.Prepend:
                    tempFile.BaseName = counter.ToString() + tempFile.BaseName;
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
                    tempFile.BaseName += counter.ToString();
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
                CreationDateTime = file.CreationDateTime.ToString()
            };
        }

        private string InsertAtBaseNameEnd() => tempFile.BaseName.Insert(tempFile.BaseName.Count(), counter.ToString());

        private string InsertAtSpecifiedIndex() => tempFile.BaseName.Insert(instructions.InsertPosition, counter.ToString());
    }
}
