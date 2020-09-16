using Microsoft.Extensions.DependencyInjection;
using Renamer;
using Renamer.Interfaces;
using Terminal.Interfaces;

namespace Terminal
{
    internal static class Program
    {
        internal static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IOrchestrate, RenameOrchestrator>()
                .AddSingleton<IBuildComposer, ComposerFactory>()
                .AddSingleton<IValidateComposeInstructions, ComposeInstructionsValidator>()
                .AddSingleton<IValidateCompositions, CompositionValidator>()
                .AddSingleton<IProvideTexts, TextProvider>()
                .AddSingleton<IRewrite, PathRewriter>()
                .BuildServiceProvider();

            var session = new Session(
                serviceProvider.GetService<IProvideTexts>(),
                serviceProvider.GetService<IOrchestrate>(),
                serviceProvider.GetService<IRewrite>()
                );

            session.Execute();
        }
    }
}
