using Renamer;
using NSubstitute;
using Xunit;
using System.Collections.Generic;

namespace RenamerTests
{
    public class RenameOrchestratorTests
    {
        readonly RenameOrchestrator subject;

        public RenameOrchestratorTests()
        {
            var inputValidator = Substitute.For<IValidateComposeInstructions>();
            var composer = Substitute.For<ICompose>();
            var outputValidator = Substitute.For<IValidateComposeInstructions>();

            subject = new RenameOrchestrator(inputValidator, composer, outputValidator);
        }

        [Fact]
        public void WhenInstantiated_ItShouldContainItsDependencies()
        {
            Assert.NotNull(subject.inputValidator);
            Assert.NotNull(subject.composer);
            Assert.NotNull(subject.outputValidator);
        }

        [Fact]
        public void WhenOrchestratingInvalidInstructions_ItShouldCallInputValidatorOnce()
        {
            var instructions = new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation>());

            _ = subject.Orchestrate(instructions);

            subject.inputValidator.Received(1).Validate(ref instructions);
        }
    }
}
