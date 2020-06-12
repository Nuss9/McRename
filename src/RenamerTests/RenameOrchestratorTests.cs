using System;
using System.Collections.Generic;
using NSubstitute;
using Renamer;
using Renamer.Interfaces;
using Xunit;

namespace RenamerTests
{
    public class RenameOrchestratorTests
    {
        private readonly IValidateComposeInstructions inputValidator;
        private readonly IBuildComposer composerFactory;
        private readonly RenameOrchestrator subject;

        public RenameOrchestratorTests()
        {
            inputValidator = Substitute.For<IValidateComposeInstructions>();
            composerFactory = Substitute.For<IBuildComposer>();
            subject = new RenameOrchestrator(inputValidator, composerFactory);
        }

        [Fact]
        public void WhenOrchestrating_ItShouldValidateInputFirst()
        {
            var invalidInstructions = GetInvalidInstructions();

            _ = subject.Orchestrate(invalidInstructions);

            inputValidator.Received(1).Validate(ref invalidInstructions);
        }
        
        [Fact]
        public void WhenOrchestratingValidInput_ItShouldComposeNext()
        {
            var instructions = GetValidInstructions();
            inputValidator.Validate(ref instructions).ReturnsForAnyArgs((true, ""));
            _ = subject.Orchestrate(instructions);

            inputValidator.Received(1).Validate(ref instructions);
            composerFactory.Received(1).Build(instructions.Mode);
        }

        [Fact]
        public void WhenOrchestratingInvalidInput_ItShouldNotCompose()
        {

        }

        [Fact]
        public void WhenOrchestrating_ItShouldValidateOutputLast()
        {

        }

        private ComposeInstructions GetInvalidInstructions() =>
            new ComposeInstructions(ComposeMode.Unknown, new List<FileInformation>());

        private ComposeInstructions GetValidInstructions() =>
            new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation> {
                    new FileInformation(
                        "/Users/JohnDoe/Desktop/fileA.txt",
                        new DateTime(2020, 01, 01, 12, 00, 00)
                    )
                }
            );
    }
}
