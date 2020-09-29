using System;
using System.Threading;
using Terminal.Interfaces;

namespace Terminal
{
    public class ConsoleWriter : IWriteToConsole
    {
        public void PrintSplashScreen()
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
    }
}
