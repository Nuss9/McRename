using System.Collections.Generic;
using ConsoleInterface.Texts;
using Microsoft.Extensions.DependencyInjection;
using Renamer;

namespace ConsoleInterface
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IBuildComposer, ComposerFactory>()
                .AddSingleton<IValidateComposeInstructions, ComposeInstructionsValidator>()
                .AddSingleton<IValidateCompositions, CompositionValidator>()
                .BuildServiceProvider();

            StandardTexts.WelcomeMessage();
            var instructions = PathRewriter.GetInstructions();

            var validator = serviceProvider.GetService<IValidateComposeInstructions>();
            var validation = validator.Validate(ref instructions);

            var composition = new Dictionary<string, string>();

            if(validation.isValid) {
                var factory = serviceProvider.GetService<IBuildComposer>();
                var composer = factory.Build(instructions.Mode);
                composition = composer.Rename(instructions);
            }
            else {
                composition.Add("Error message", validation.errorMessage);
            }

            var compositionValidator = serviceProvider.GetService<IValidateCompositions>();
            var compositionValidation = compositionValidator.Validate(composition);

            if(compositionValidation.isValid)
            {
                PathRewriter.Rewrite(composition);
            }
            else
            {
                composition.Clear();
                composition.Add("Error message", compositionValidation.errorMessage);
                PathRewriter.Rewrite(composition);
            }

            StandardTexts.Finished();
        }
    }
}
