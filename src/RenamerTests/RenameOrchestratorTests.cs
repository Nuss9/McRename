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
        private readonly IValidateCompositions outputValidator;
        private readonly RenameOrchestrator subject;

        public RenameOrchestratorTests()
        {
            inputValidator = Substitute.For<IValidateComposeInstructions>();
            composerFactory = Substitute.For<IBuildComposer>();
            outputValidator = Substitute.For<IValidateCompositions>();
            subject = new RenameOrchestrator(inputValidator, composerFactory, outputValidator);
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
            var invalidInstructions = GetInvalidInstructions();
            inputValidator.Validate(ref invalidInstructions).Returns((false, "Unknown compose mode."));

            _ = subject.Orchestrate(invalidInstructions);

            inputValidator.Received(1).Validate(ref invalidInstructions);
            composerFactory.Received(0).Build(Arg.Any<ComposeMode>());
        }

        [Fact]
        public void WhenOrchestrating_ItShouldValidateOutputLast()
        {
            var invalidInstructions = GetValidInstructions();
            inputValidator.Validate(ref invalidInstructions).Returns((true, ""));

            _ = subject.Orchestrate(invalidInstructions);

            inputValidator.Received(1).Validate(ref invalidInstructions);
            composerFactory.Received(1).Build(Arg.Any<ComposeMode>());
            outputValidator.Received(1).Validate(Arg.Any<Dictionary<string, string>>());
        }

        [Fact]
        public void WhenInputValidatorFindsAnError_ItShouldBeReturnedByOrchestrator()
        {
            var expected = new Dictionary<string, string> {{"Error", "Unknown compose mode."}};

            var invalidInstructions = GetInvalidInstructions();
            inputValidator.Validate(ref invalidInstructions).Returns((false, "Unknown compose mode."));

            var result = subject.Orchestrate(invalidInstructions);

            inputValidator.Received(1).Validate(ref invalidInstructions);
            composerFactory.Received(0).Build(Arg.Any<ComposeMode>());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenOutputValidatorFindsAnError_ItShouldBeReturnedByOrchestrator()
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
