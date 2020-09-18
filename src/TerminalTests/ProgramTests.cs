﻿using System;
using System.IO;
using Terminal;
using Xunit;

namespace TerminalTests
{
    public class ProgramTests
    {
        private readonly string testDirectoryPath;
        private readonly string testFileAPath;
        private readonly string testFileBPath;
        private readonly string testFileCPath;

        public ProgramTests()
        {
            testDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestFolder");
            testFileAPath = Path.Combine(testDirectoryPath, "fileA.png");
            testFileBPath = Path.Combine(testDirectoryPath, "fileB.png");
            testFileCPath = Path.Combine(testDirectoryPath, "fileC.png");

            Directory.CreateDirectory(testDirectoryPath);
            var streamA = File.Create(testFileAPath);
            streamA.Close();
            var streamB = File.Create(testFileBPath);
            streamB.Close();
            var streamC = File.Create(testFileCPath);
            streamC.Close();
        }

        [Fact]
        public void A_WhenStartingProgramTests_ItShouldCreateTempTestFiles()
        {
            Assert.True(File.Exists(testFileAPath));
            Assert.True(File.Exists(testFileBPath));
            Assert.True(File.Exists(testFileCPath));
        }

        [Fact]
        public void B_WhenExecuting_ItShouldAlwaysPrintSplashScreen()
        {
            using StringWriter sw = new StringWriter();
            using StringReader sr = new StringReader(string.Format("TestFolder{0}1{0}Y{0}n{0}", Environment.NewLine));

            Console.SetOut(sw);
            Console.SetIn(sr);

            Program.Main();

            Assert.Contains("- - Batch rename files from a Desktop directory - -", sw.ToString());
        }

        [Fact]
        public void C_WhenFinishingProgramTests_ItShouldDeleteTempTestFiles()
        {
            var paths = Directory.GetFiles(testDirectoryPath);
            foreach(string path in paths)
            {
                File.Delete(path);
            }

            Directory.Delete(testDirectoryPath);

            Assert.False(File.Exists(testFileAPath));
            Assert.False(File.Exists(testFileBPath));
            Assert.False(File.Exists(testFileCPath));
            Assert.False(Directory.Exists(testDirectoryPath));
        }
    }
}
