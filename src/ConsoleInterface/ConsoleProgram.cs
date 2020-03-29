using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Renamer;

namespace ConsoleInterface
{
    public static class ConsoleProgram
    {
        public static RenameInstructions GetInstructions()
        {
            string targetDirectory = RequestDirectory();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            var filesInformation = new List<FileInformation>();

            foreach(string file in fileEntries) {
                filesInformation.Add(
                    new FileInformation(
                        file,
                        Path.GetExtension(file),
                        File.GetCreationTime(file)
                    )
                );
            };

			RenameMode mode = RenameMode.Unknown;

			while(mode == RenameMode.Unknown) {
				mode = RequestMode();
			}

            return new RenameInstructions(mode, filesInformation);
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
					SimulateWaitingWithMessage("Input directory not found. Try again");
                    Console.WriteLine("");
                }
            }
		}

		private static RenameMode RequestMode()
        {
            Console.WriteLine("Specify the format of the new filenames:");
            Console.WriteLine("1) Numerical");
            Console.WriteLine("2) Date_Time: YYYYMMDD_HHMMSS");
            Console.WriteLine("3) Date: YYYMMDD");
            Console.WriteLine("------------");
            Console.Write("  Mode: ");

            string mode = Console.ReadLine();

            Console.WriteLine("");

			return (int.Parse(mode)) switch
			{
                1 => RenameMode.Numerical,
				_ => RenameMode.Unknown,
			};
		}

		private static void SimulateWaitingWithMessage(string message)
		{
			Thread.Sleep(500);
			Console.Write($"{message}");
			Thread.Sleep(500);
			Console.Write(".");
			Thread.Sleep(500);
			Console.Write(".");
			Thread.Sleep(500);
			Console.Write(".");
			Console.WriteLine("");
			Thread.Sleep(2000);
		}

		internal static void Finished()
		{
			Console.WriteLine("----------------------------");
			Console.WriteLine("      Finished.");
			Console.WriteLine("");
			SimulateWaitingWithMessage("Closing application");
			Environment.Exit(0);
		}
    }
}