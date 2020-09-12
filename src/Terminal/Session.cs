using Microsoft.Extensions.DependencyInjection;
using Renamer;
using Renamer.Interfaces;
using Terminal.Texts;

namespace Terminal
{
    internal class Session
    {
        public Session()
        {
        }

        public void Execute()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IOrchestrate, RenameOrchestrator>()
                .AddSingleton<IBuildComposer, ComposerFactory>()
                .AddSingleton<IValidateComposeInstructions, ComposeInstructionsValidator>()
                .AddSingleton<IValidateCompositions, CompositionValidator>()
                .BuildServiceProvider();


            StandardTexts.WelcomeMessage();

            bool repeat = true;
            var orchestrator = serviceProvider.GetService<IOrchestrate>();

            while (repeat)
            {
                var instructions = QuestionTexts.GetInstructions();

                var composition = orchestrator.Orchestrate(instructions);

                PathRewriter.Rewrite(composition);

                repeat = QuestionTexts.AskForBoolean("Continue renaming?");
            }

            StandardTexts.Finished();
        }
    }
}
