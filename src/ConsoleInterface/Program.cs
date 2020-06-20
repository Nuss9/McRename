using ConsoleInterface.Texts;
using Microsoft.Extensions.DependencyInjection;
using Renamer;
using Renamer.Interfaces;

namespace ConsoleInterface
{
    internal static class Program
    {
        private static void Main(string[] args)
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

            while(repeat)
            {
                var instructions = QuestionTexts.GetInstructions();

                var composition = orchestrator.Orchestrate(instructions);

                PathRewriter.Rewrite(composition);

                repeat = QuestionTexts.RenameAgain();
            }


            StandardTexts.Finished();
        }
    }
}
