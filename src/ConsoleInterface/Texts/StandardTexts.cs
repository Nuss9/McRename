using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConsoleInterface.Texts
{
    public static class StandardTexts
    {
        public static void SimulateWaitingWithMessage(string message)
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

		public static void Finished()
		{
			Console.WriteLine("----------------------------");
			Console.WriteLine("      Finished.");
			Console.WriteLine("");
			SimulateWaitingWithMessage("Closing application");
			Environment.Exit(0);
		}

        public static void ProposeFilenameChanges(Dictionary<string, string> proposal)
        {
            Console.WriteLine("   Proposing the following name changes:");
			Console.WriteLine("");
            Console.WriteLine(" -------------------------------------------------------------");
			Console.WriteLine("{0, -2} {1, -26} {2, -3} {3, -26} {4, 2}", "|",  "Original file names", "", "New file names", "|");
            Console.WriteLine(" -------------------------------------------------------------");

            foreach(KeyValuePair<string, string> pair in proposal) {
                string originalName = Path.GetFileName(pair.Key);
                string proposedName = Path.GetFileName(pair.Value);
				Console.WriteLine("{0, -2} {1, -26} {2, -3} {3, -26} {4, 2}", "|",  originalName, " > ", proposedName, "|");
            }

            Console.WriteLine(" -------------------------------------------------------------");
			Console.WriteLine("");
        }
    }
}