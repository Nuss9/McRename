using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ConsoleInterface.Texts;
using Renamer;

namespace ConsoleInterface
{
    public static class PathRewriter
    {
        public static RenameInstructions GetInstructions()
        {
            string targetDirectory = QuestionTexts.RequestDirectory();
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

			ComposeMode mode = ComposeMode.Unknown;

			while(mode == ComposeMode.Unknown) {
				mode = QuestionTexts.RequestMode();
			}

            return new RenameInstructions(mode, filesInformation);
        }
        public static void Rewrite(Dictionary<string, string> proposal)
		{
			if(proposal.TryGetValue("Error message", out string message)) {
				StandardTexts.DisplayError(message);
				StandardTexts.Finished();
			}

            StandardTexts.ProposeFilenameChanges(proposal);

            var execute = QuestionTexts.AskPermission();

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
    }
}
