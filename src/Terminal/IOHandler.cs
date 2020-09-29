using Terminal.Interfaces;

namespace Terminal
{
    public class IOHandler
    {
        private readonly IWriteToConsole consoleWriter;
        private readonly IReadFromConsole consoleReader;

        public IOHandler(IWriteToConsole consoleWriter, IReadFromConsole consoleReader)
        {
            this.consoleWriter = consoleWriter;
            this.consoleReader = consoleReader;
        }
    }
}
