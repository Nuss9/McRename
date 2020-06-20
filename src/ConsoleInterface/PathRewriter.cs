using System.Collections.Generic;
using System.IO;
using ConsoleInterface.Texts;

namespace ConsoleInterface
{
    public static class PathRewriter
    {
        internal static void Rewrite(Dictionary<string, string> proposal)
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
