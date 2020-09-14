using Renamer.Dto;
using Terminal.Interfaces;

namespace Terminal
{
    public class TextProvider : IProvideTexts
    {
        public void WelcomeMessage()
        {
            Texts.StandardTexts.WelcomeMessage();
        }

        public ComposeInstructions GetInstructions()
        {
            return Texts.QuestionTexts.GetInstructions();
        }

        public bool AskForBoolean(string question)
        {
            return Texts.QuestionTexts.AskForBoolean(question);
        }

        public void Finished()
        {
            Texts.StandardTexts.Finished();
        }
    }
}
