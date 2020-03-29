using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConsoleInterface
{
    public static class PathRewriter
    {
        public static void Rewrite(Dictionary<string, string> proposal) {
            
            Console.WriteLine("Proposing the following name changes:");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");

            foreach(KeyValuePair<string, string> pair in proposal) {
                string original = Path.GetFileName(pair.Key);
                string proposed = Path.GetFileName(pair.Value);
                
                Console.WriteLine($"{original} > {proposed}");
            }

            Console.WriteLine("");
            Console.WriteLine("-------------------------------------");

            var execute = AskPermission();

            if(execute) {
                RewriteFilePaths(proposal);
            }

            Console.WriteLine("");
            Console.WriteLine("Program finished.");
            Console.WriteLine("Exiting...");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        private static void RewriteFilePaths(Dictionary<string, string> proposal)
        {
            foreach(KeyValuePair<string, string> pair in proposal) {
                File.Move(pair.Key, pair.Value);
            }
        }

        private static bool AskPermission()
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

                Console.WriteLine("Error parsing input. Retry.");
            }
        }
    }
}
