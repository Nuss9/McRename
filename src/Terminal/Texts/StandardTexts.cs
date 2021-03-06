using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Terminal.Texts
{
	internal static class StandardTexts
    {
		internal static void WelcomeMessage()
        {
			Console.WriteLine("");
			Console.WriteLine("   * — — — — — — — — — — — — — — — — — — — — — — — — — — — *");
			Console.WriteLine("   |                __                                     |");
			Console.WriteLine("   |  |\\   /|      |  \\                                    |");
			Console.WriteLine("   |  | \\ / |      |   |                                   |");
			Console.WriteLine("   |  |     |   _  |__/    _                 _             |");
			Console.WriteLine("   |  |     |  /   |  \\   |_ |\\ |  /\\  |\\/| |_             |");
			Console.WriteLine("   |  |     |  \\_  |   \\  |_ | \\| /--\\ |  | |_             |");
			Console.WriteLine("   |                                                       |");
			Console.WriteLine("   * — — — — — — — — — — — — — — — — — — — — — — — — — — — *");
			Console.WriteLine("");
			Thread.Sleep(1500);
			Console.WriteLine("      - - Batch rename files from a Desktop directory - - ");
			Console.WriteLine("");
			Console.WriteLine("");
			Thread.Sleep(1500);
		}


		internal static void SimulateWaitingWithMessage(string message)
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

		internal static void DisplayError(string errorMessage)
		{
			SimulateWaitingWithMessage("An error occurred");
			Console.WriteLine(errorMessage);
			Console.WriteLine("");
			Console.WriteLine("Renaming aborted.");
			Console.WriteLine("");
		}

		internal static void SuccessfullRewrite()
        {
			Console.WriteLine("");
			Console.WriteLine("Successfully changed file names.");
			Thread.Sleep(2000);
		}

		internal static void Finished()
		{
			Console.WriteLine("----------------------------");
			Console.WriteLine("      Finished.");
			Console.WriteLine("");
			SimulateWaitingWithMessage("Closing application");
		}

		internal static void ProposeFilenameChanges(Dictionary<string, string> proposal)
        {
            Console.WriteLine("   Proposing the following name changes:");
			Console.WriteLine("");
            Console.WriteLine(" -------------------------------------------------------------");
			Console.WriteLine("{0, -2} {1, -26} {2, -3} {3, -26} {4, 2}", "|",  "Original file names", "", "New file names", "|");
            Console.WriteLine(" -------------------------------------------------------------");

            foreach(KeyValuePair<string, string> pair in proposal) {
                string originalName = Path.GetFileName(pair.Key);

                if(originalName.Length > 24)
                {
					originalName = originalName.Substring(0, 24) + "..";
                }

                string proposedName = Path.GetFileName(pair.Value);
				Console.WriteLine("{0, -2} {1, -26} {2, -3} {3, -26} {4, 2}", "|",  originalName, " > ", proposedName, "|");
            }

            Console.WriteLine(" -------------------------------------------------------------");
			Console.WriteLine("");
        }
    }
}