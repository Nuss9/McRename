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
            var instructions = PathRewriter.GetInstructions();

            var orchestrator = serviceProvider.GetService<IOrchestrate>();
            var composition = orchestrator.Orchestrate(instructions);

            PathRewriter.Rewrite(composition);

            StandardTexts.Finished();
        }
    }
}
