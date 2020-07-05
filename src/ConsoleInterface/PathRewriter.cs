using System.Collections.Generic;
using System.IO;
using static Terminal.Texts.QuestionTexts;
using static Terminal.Texts.StandardTexts;

namespace Terminal
{
    public static class PathRewriter
    {
        internal static void Rewrite(Dictionary<string, string> proposal)
        {
            CheckForErrorMessage(proposal);

            ProposeFilenameChanges(proposal);

            if (AskForBoolean("Accept the proposed name changes?"))
            {
                RewriteFilePaths(proposal);
                SuccessfullRewrite();
            }
        }

        private static void CheckForErrorMessage(Dictionary<string, string> proposal)
        {
            if (proposal.TryGetValue("Error", out string message))
            {
                DisplayError(message);
                Finished();
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
