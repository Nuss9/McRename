using System.Collections.Generic;
using Renamer.Interfaces;

namespace Renamer
{
    public class RenameOrchestrator : IOrchestrate
    {
        private readonly IValidateComposeInstructions inputValidator;
        private readonly IBuildComposer composerFactory;
        private readonly IValidateCompositions outputValidator;
        private ICompose composer;

        public RenameOrchestrator(IValidateComposeInstructions inputValidator, IBuildComposer composerFactory, IValidateCompositions outputValidator)
        {
            this.inputValidator = inputValidator;
            this.composerFactory = composerFactory;
            this.outputValidator = outputValidator;
        }

        public Dictionary<string, string> Orchestrate(ComposeInstructions instructions)
        {
            var inputValidation = inputValidator.Validate(ref instructions);

            if (!inputValidation.isValid)
            {
                return Error(inputValidation.errorMessage);
            }

            composer = composerFactory.Build(instructions.Mode);

            var composition = composer.Compose(instructions);

            var outputValidation = outputValidator.Validate(composition);

            if (!outputValidation.isValid)
            {
                return Error(outputValidation.errorMessage);
            }

            return composition;
        }

        private static Dictionary<string, string> Error(string errorMessage) =>
            new Dictionary<string, string> { { "Error", errorMessage } };
    }
}
