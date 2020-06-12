using System.Collections.Generic;
using NSubstitute;
using Renamer;
using Renamer.Interfaces;
using Xunit;

namespace RenamerTests
{
    public class RenameOrchestratorTests
    {
        [Fact]
        public void WhenOrchestrating_ItShouldValidateInputFirst()
        {
            var inputValidator = Substitute.For<IValidateComposeInstructions>();
            var orchestrator = new RenameOrchestrator(inputValidator);

            var instructions = new ComposeInstructions(
                    ComposeMode.Unknown,
                    new List<FileInformation>()
                );

            _ = orchestrator.Orchestrate(instructions);

            inputValidator.Received(1).Validate(ref instructions);
        }

        [Fact]
        public void WhenOrchestratingValidInput_ItShouldComposeNext()
        {

        }

        [Fact]
        public void WhenOrchestratingInvalidInput_ItShouldNotCompose()
        {

        }

        [Fact]
        public void WhenOrchestrating_ItShouldValidateOutputLast()
        {

        }
    }
}
