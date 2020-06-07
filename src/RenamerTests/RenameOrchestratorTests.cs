using Renamer;
using NSubstitute;
using Xunit;

namespace RenamerTests
{
    public class RenameOrchestratorTests
    {
        [Fact]
        public void WhenInstantiated_ItShouldContainItsDependencies()
        {
            var inputValidator = Substitute.For<IValidateComposeInstructions>();
            var composer = Substitute.For<ICompose>();
            var outputValidator = Substitute.For<IValidateComposeInstructions>();

            var subject = new RenameOrchestrator(inputValidator, composer, outputValidator);

            Assert.NotNull(subject.inputValidator);
            Assert.NotNull(subject.composer);
            Assert.NotNull(subject.outputValidator);
        }
    }
}
