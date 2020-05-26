using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Xunit;

namespace RenamerTests
{
    public class ComposeInstructionsValidatorTests
    {
        public ComposeInstructionsValidator validator;
        readonly char s;

        public ComposeInstructionsValidatorTests()
        {
            s = Path.DirectorySeparatorChar;
            validator = new ComposeInstructionsValidator();
        }

        [Fact]
        public void WhenInstructionsContainHiddenFiles_ItShouldRemoveThose()
        {
            var instructions = new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation> {
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", new DateTime(2020, 01, 01, 12, 01, 01)),
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}.fileA.txt", new DateTime(2020, 01, 01, 12, 01, 02)),
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", new DateTime(2020, 01, 01, 12, 01, 03)),
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}.fileB.txt", new DateTime(2020, 01, 01, 12, 01, 04))
            });

            var expected = new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation> {
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", new DateTime(2020, 01, 01, 12, 01, 01)),
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", new DateTime(2020, 01, 01, 12, 01, 03)),
            });

            validator.RemoveHiddenFiles(ref instructions);

            Assert.True(instructions.Equals(expected));
        }
    }
}