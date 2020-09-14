using Renamer.Dto;

namespace Terminal.Interfaces
{
    public interface IProvideTexts
    {
        void WelcomeMessage();
        ComposeInstructions GetInstructions();
        bool AskForBoolean(string question);
        void Finished();
    }
}
