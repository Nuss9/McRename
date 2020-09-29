using System;
using Terminal.Interfaces;

namespace Terminal
{
    public class IOHandler : IHandleIO
    {
        private readonly IWriteToConsole consoleWriter;
        private readonly IReadFromConsole consoleReader;

        public IOHandler(IWriteToConsole consoleWriter, IReadFromConsole consoleReader)
        {
            this.consoleWriter = consoleWriter;
            this.consoleReader = consoleReader;
        }

        public void ExecuteSplashScreen()
        {
            consoleWriter.PrintSplashScreen();
        }
    }
}
