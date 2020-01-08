using System;
using System.IO;
using System.Threading;

namespace ConsoleInterface
{
    internal class ConsoleTexts
    {
        public (int, string) Execute()
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
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
        private static string RequestDirectory()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            while(true) {
                Console.Write("Please provide the desktop folder to rename its content: ");
                string inputDirectory = Console.ReadLine();
                string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + inputDirectory;
                if(Directory.Exists(fullPath)) {
                    return fullPath;
                }
                else {
                    Thread.Sleep(500);
                    Console.Write("Input directory not found. Try again");
                    Thread.Sleep(500);
                    Console.Write(".");
                    Thread.Sleep(500);
                    Console.Write(".");
                    Thread.Sleep(500);
                    Console.Write(".");
                    Thread.Sleep(2000);
                    Console.Clear();
                }
            }        }

        private int RequestMode()
        {
            Console.WriteLine("Specify the format of the new filenames:");
            Console.WriteLine("1) YYYYMMDD_HHMMSS");
            Console.WriteLine("2) YYYMMDD");
            
            string mode = Console.ReadLine();

            return int.Parse(mode);
        }
    }
}