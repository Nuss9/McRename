using System;
using System.Collections.Generic;
using System.IO;
using Renamer;

namespace ConsoleInterface
{
    public static class ConsoleProgram
    {
        public static RenameInstructionsDto GetInstructions()
        {
            string targetDirectory = RequestDirectory();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            var filesInformation = new List<FileInfoDto>();

            foreach(string file in fileEntries) {
                filesInformation.Add(
                    new FileInfoDto(
                        file,
                        Path.GetExtension(file),
                        File.GetCreationTime(file)
                    )
                );
            };

            int mode = RequestMode();

            return new RenameInstructionsDto(mode, filesInformation);
        }

        private static string RequestDirectory()
        {
            while(true) {
                Console.Write("Please provide the desktop folder to rename its content: ");
                string inputDirectory = Console.ReadLine().Replace(" ", "\u0020");
                string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + inputDirectory;

                if(Directory.Exists(fullPath)) {
                    return fullPath;
                }
                else {
					ConsoleTexts.SimulateWaitingWithMessage("Input directory not found. Try again");
                    Console.WriteLine("");
                }
            }
		}

		private static int RequestMode()
        {
            Console.WriteLine("Specify the format of the new filenames:");
            Console.WriteLine("1) YYYYMMDD_HHMMSS");
            Console.WriteLine("2) YYYMMDD");
            Console.WriteLine("------------");
            Console.Write("  Mode: ");

            string mode = Console.ReadLine();

            Console.WriteLine("");

            return int.Parse(mode);
        }
    }
}