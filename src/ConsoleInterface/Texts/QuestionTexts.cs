using System;
using System.IO;
using Renamer;

namespace ConsoleInterface.Texts
{
    public class QuestionTexts
    {
        public static string RequestDirectory()
        {
            while(true) {
                Console.Write("Please provide the desktop folder to rename its content: ");
                string inputDirectory = Console.ReadLine().Replace(" ", "\u0020");
                string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + inputDirectory;

                if(Directory.Exists(fullPath)) {
                    return fullPath;
                }
                else {
					StandardTexts.SimulateWaitingWithMessage("Input directory not found. Try again");
                    Console.WriteLine("");
                }
            }
		}

        public static ComposeMode RequestMode()
        {
            while (true)
            {
                Console.WriteLine("Specify the format of the new filenames:");
                Console.WriteLine("1) Numerical");
                Console.WriteLine("2) Date: YYYMMDD");
                Console.WriteLine("3) Date_Time: YYYYMMDD_HHMMSS");
                Console.WriteLine("4) Custom text");
                Console.WriteLine("5) Trucation");
                Console.WriteLine("------------");
                Console.Write("  Mode: ");
                string input = Console.ReadLine();
                Console.WriteLine("");

                if (int.TryParse(input, out int mode))
                {
                    return (mode) switch
                    {
                        1 => ComposeMode.Numerical,
                        2 => ComposeMode.Date,
                        3 => ComposeMode.DateTime,
                        4 => ComposeMode.CustomText,
                        5 => ComposeMode.Truncation,
                        _ => ComposeMode.Unknown,
                    };
                }
                else
                {
                    Console.WriteLine("");
                    Console.Write("That is not a valid option. ");
                    StandardTexts.SimulateWaitingWithMessage("Retry");
                    Console.WriteLine("");
                }
            }
		}

        internal static string RequestCustomText(ComposeMode mode)
        {
            string text;
            string requestText;

            if(mode == ComposeMode.CustomText)
            {
                requestText = "Please specify the custom text for all files: ";
            }
            else
            {
                requestText = "Please specify the text to truncate from filenames: ";
            }

            while(true)
            {
                Console.Write(requestText);
                text = Console.ReadLine();
                Console.WriteLine("");

                if(string.IsNullOrWhiteSpace(text)) {
                    Console.Write("Input invalid. ");
                }
                else if(text.Length > 15) {
                    Console.Write("Input too long (max. 15 characters).");
                }
                else
                {
                    return text;
                }

                StandardTexts.SimulateWaitingWithMessage("Retry");
            }

        }

        public static bool AskPermission()
        {
            while(true) {
                Console.WriteLine("");
                Console.Write("Accept the proposed name changes (Y/n)? ");
                var answer = Console.ReadLine().ToLower();

                if(answer == string.Empty || answer == "y") {
                    return true;
                }
                else if(answer == "n") {
                    return false;
                }

                StandardTexts.SimulateWaitingWithMessage("Error parsing input. Retry.");
            }
        }
    }
}
