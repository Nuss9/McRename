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

            validator.Validate(ref instructions);

            Assert.True(instructions.Equals(expected));
        }
    
        [Fact]
        public void WhenComposeModeIsUnknown_ItShouldReturnAnError()
        {
            var instructions = new ComposeInstructions(ComposeMode.Unknown, new List<FileInformation> {
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", new DateTime(2020, 01, 01, 12, 01, 01)),
                    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", new DateTime(2020, 01, 01, 12, 01, 03)),
            });

            var result = validator.Validate(ref instructions);

            var expected = (false, "Compose mode unknown.");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenFilesListIsEmpty_ItShouldReturnAnError()
        {
            var instructions = new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation>());

            var expected = (false, "No files found in selected directory.");

            var result = validator.Validate(ref instructions);

            Assert.Equal(expected, result);
        }
    }
}
