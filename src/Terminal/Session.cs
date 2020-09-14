using Microsoft.Extensions.DependencyInjection;
using Renamer;
using Renamer.Interfaces;
using Terminal.Texts;
using Terminal.Interfaces;

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
                .AddSingleton<IProvideTexts, TextProvider>()
                .AddSingleton<IRewrite, PathRewriter>()
                .BuildServiceProvider();

            var textProvider = serviceProvider.GetService<IProvideTexts>();
            textProvider.WelcomeMessage();

            var orchestrator = serviceProvider.GetService<IOrchestrate>();
            var rewriter = serviceProvider.GetService<IRewrite>();
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
