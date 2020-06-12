using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Renamer.Interfaces;

namespace Renamer
{
    public class RenameOrchestrator : IOrchestrate
    {
        private IValidateComposeInstructions inputValidator;

        public RenameOrchestrator(IValidateComposeInstructions inputValidator)
        {
            this.inputValidator = inputValidator;
        }

        public Dictionary<string, string> Orchestrate(ComposeInstructions instructions)
        {
            var composition = new Dictionary<string, string>();

            _ = inputValidator.Validate(ref instructions);

            return composition;
        }
    }
}
