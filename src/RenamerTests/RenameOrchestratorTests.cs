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
            var validInstructions = GetValidInstructions();

            inputValidator.Validate(ref validInstructions).ReturnsForAnyArgs((true, ""));
            _ = subject.Orchestrate(validInstructions);

            inputValidator.Received(1).Validate(ref validInstructions);
            composerFactory.Received(1).Build(validInstructions.Mode);
            // Check also composer.Received(1).Compose(instructions);
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
