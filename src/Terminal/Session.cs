using Renamer.Interfaces;
using Terminal.Interfaces;

namespace Terminal
{
    internal class Session
    {
        private readonly IProvideTexts textProvider;
        private readonly IOrchestrate orchestrator;
        private readonly IRewrite rewriter;

        public Session(IProvideTexts textProvider, IOrchestrate orchestrator, IRewrite rewriter)
        {
            this.textProvider = textProvider;
            this.orchestrator = orchestrator;
            this.rewriter = rewriter;
        }

        public void Execute()
        {
            textProvider.WelcomeMessage();

            bool repeat = true;

            while (repeat)
            {
                var instructions = textProvider.GetInstructions();

                var composition = orchestrator.Orchestrate(instructions);

                rewriter.Rewrite(composition);

                repeat = textProvider.AskForBoolean("Continue renaming?");
            }

            textProvider.Finished();
        }
    }
}
