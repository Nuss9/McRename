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
            var composition = new Dictionary<string, string>();

            var inputValidation = inputValidator.Validate(ref instructions);

            if (!inputValidation.isValid)
            {
                composition.Add("Error", inputValidation.errorMessage);
                
                return composition;
            }

            composer = composerFactory.Build(instructions.Mode);
            
            composition = composer.Compose(instructions);

            var outputValidation = outputValidator.Validate(composition);

            if (!outputValidation.isValid)
            {
                return composition;
                // Validate error message!
            }

            return composition;
        }
    }
}
