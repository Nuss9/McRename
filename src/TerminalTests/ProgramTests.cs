using System;
using System.IO;
using Terminal;
using Xunit;

namespace TerminalTests
{
    public class ProgramTests
    {
        [Fact]
        public void WhenExecuting_ItShouldAlwaysPrintSplashScreen()
        {
            using StringWriter sw = new StringWriter();
            using StringReader sr = new StringReader(string.Format("pics{0}1{0}Y{0}n{0}", Environment.NewLine));

            Console.SetOut(sw);
            Console.SetIn(sr);

            Program.Main();

            Assert.Contains("- - Batch rename files from a Desktop directory - -", sw.ToString());
        }
    }
}
