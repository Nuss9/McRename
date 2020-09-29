using Terminal;
using Terminal.Interfaces;
using NSubstitute;
using Xunit;

namespace TerminalTests
{
    public class IOHandlerTests
    {
        private readonly IWriteToConsole consoleWriter;
        private readonly IReadFromConsole consoleReader;
        private readonly IOHandler subject;

        public IOHandlerTests()
        {
            consoleWriter = Substitute.For<IWriteToConsole>();
            consoleReader = Substitute.For<IReadFromConsole>();
            subject = new IOHandler(consoleWriter, consoleReader);
        }

        [Fact]
        public void When_ItShould()
        {

        }
    }
}
