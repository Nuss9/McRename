using System.Collections.Generic;
using Renamer.Interfaces;

namespace Renamer
{
    public class RenameOrchestrator : IOrchestrate
    {
        public IValidateComposeInstructions inputValidator;
        public object composer;         // Needs to be private
        public object outputValidator; // Needs to be private

        public RenameOrchestrator(
            IValidateComposeInstructions inputValidator,
            ICompose composer,
            IValidateComposeInstructions outputValidator
            )
        {
            this.inputValidator = inputValidator;
            this.composer = composer;
            this.outputValidator = outputValidator;
        }

        public Dictionary<string, string> Orchestrate(ComposeInstructions instructions)
        {
            var composition = new Dictionary<string, string>();

            var validation = inputValidator.Validate(ref instructions);

            return composition;
        }
    }
}
