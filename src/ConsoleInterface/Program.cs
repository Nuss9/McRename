using System.Collections.Generic;
using ConsoleInterface.Texts;
using Microsoft.Extensions.DependencyInjection;
using Renamer;
using Renamer.Composers;

namespace ConsoleInterface
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IRename, FileNameComposer>()
                .AddSingleton<IValidateComposeInstructions, ComposeInstructionsValidator>()
                .AddSingleton<IValidateCompositions, CompositionValidator>()
                .BuildServiceProvider();

            StandardTexts.WelcomeMessage();
            var instructions = PathRewriter.GetInstructions();

            var validator = serviceProvider.GetService<IValidateComposeInstructions>();
            var validation = validator.Validate(ref instructions);

            var proposal = new Dictionary<string, string>();

            if(validation.isValid) {
                var renamer = serviceProvider.GetService<IRename>();
			    proposal = renamer.Rename(instructions);
            }
            else {
                proposal.Add("Error message", validation.errorMessage);
            }

            var compositionValidator = serviceProvider.GetService<IValidateCompositions>();
            var compositionValidation = compositionValidator.Validate(proposal);

            if(!compositionValidation.isValid)
            {
                PathRewriter.Rewrite(proposal);
            }
            else
            {
                proposal.Clear();
                proposal.Add("Error message", compositionValidation.errorMessage);
                PathRewriter.Rewrite(proposal);
            }

            StandardTexts.Finished();
        }
    }
}
