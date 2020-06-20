using System.Collections.Generic;
using System.IO;
using ConsoleInterface.Texts;
using Renamer;

namespace ConsoleInterface
{
    public static class PathRewriter
    {
        public static ComposeInstructions GetInstructions()
        {
            string targetDirectory = QuestionTexts.RequestDirectory();
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            var filesInformation = new List<FileInformation>();

            foreach(string file in fileEntries) {
                filesInformation.Add(
                    new FileInformation(
                        file,
                        File.GetCreationTime(file)
                    )
                );
            };

            ComposeMode mode = ComposeMode.Unknown;

            while (mode == ComposeMode.Unknown) {
                mode = QuestionTexts.RequestMode();
            }

            if (mode == ComposeMode.CustomText || mode == ComposeMode.Truncation || mode == ComposeMode.Extension) {
                string customText = QuestionTexts.RequestCustomText(mode);
                return new ComposeInstructions(mode, customText, filesInformation);
            }

            return new ComposeInstructions(mode, filesInformation);
        }

        public static void Rewrite(Dictionary<string, string> proposal)
        {
            if (proposal.TryGetValue("Error message", out string message)) {
                StandardTexts.DisplayError(message);
                StandardTexts.Finished();
            }

            StandardTexts.ProposeFilenameChanges(proposal);

            var execute = QuestionTexts.AskPermission();

            if(execute) {
                RewriteFilePaths(proposal);
                StandardTexts.SuccessfullRewrite();
            }
        }

        private static void RewriteFilePaths(Dictionary<string, string> proposal)
        {
            foreach(KeyValuePair<string, string> pair in proposal) {
                File.Move(pair.Key, pair.Value);
            }
        }
    }
}
