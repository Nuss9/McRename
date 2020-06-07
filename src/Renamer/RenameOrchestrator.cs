using System.Runtime.CompilerServices;

namespace Renamer
{
    public class RenameOrchestrator
    {
        public object inputValidator;    // Needs to be private
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
    }
}
