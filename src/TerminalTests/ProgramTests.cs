using System;
using System.IO;
using Terminal;
using Xunit;

namespace TerminalTests
{
    public class ProgramTests : IDisposable
    {
        private readonly string testDirectoryPath;
        public ProgramTests()
        {
            testDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestFolder");
            string testFileAPath = Path.Combine(testDirectoryPath, "fileA.png");
            string testFileBPath = Path.Combine(testDirectoryPath, "fileB.png");
            string testFileCPath = Path.Combine(testDirectoryPath, "fileC.png");

            Directory.CreateDirectory(testDirectoryPath);
            File.Create(testFileAPath);
            File.Create(testFileBPath);
            File.Create(testFileCPath);
        }

        [Fact]
        public void SingleSession_ShouldPrintWelcomeOnce()
        {
            using StringWriter sw = new StringWriter();
            using StringReader sr = new StringReader(string.Format("TestFolder{0}1{0}Y{0}n{0}", Environment.NewLine));

            Console.SetOut(sw);
            Console.SetIn(sr);

            Program.Main();

            Assert.Contains("- - Batch rename files from a Desktop directory - -", sw.ToString());
        }

        [Fact]
        public void DoubleSession_ShouldPrintWelcomeOnce()
        {
            using StringWriter sw = new StringWriter();
            using StringReader sr = new StringReader(string.Format("TestFolder{0}1{0}Y{0}Y{0}TestFolder{0}2{0}Y{0}n{0}", Environment.NewLine));

            Console.SetOut(sw);
            Console.SetIn(sr);

            Program.Main();

            Assert.Contains("- - Batch rename files from a Desktop directory - -", sw.ToString());
        }

        public void Dispose()
        {
            var paths = Directory.GetFiles(testDirectoryPath);
            foreach (string path in paths)
            {
                File.Delete(path);
            }

            Directory.Delete(testDirectoryPath);
        }
    }
}
