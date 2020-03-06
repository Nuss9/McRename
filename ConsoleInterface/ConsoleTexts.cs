using System;
using System.IO;
using System.Threading;

namespace ConsoleInterface
{
    internal static class ConsoleTexts
    {
        public static (int, string) Execute()
        {
            Console.Clear();
            Console.Title = "- - - Batch renamer - - -";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            int mode = RequestMode();
            string path = RequestDirectory();

            return (mode, path);
        }

        public static void Finished()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("      Finished.");
			Console.WriteLine("");
            SimulateWaitingWithMessage("Closing application");
            Environment.Exit(0);
        }
        private static string RequestDirectory()
        {
            while(true) {
                Console.Write("Please provide the desktop folder to rename its content: ");
                string inputDirectory = Console.ReadLine();
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