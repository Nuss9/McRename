using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Xunit;

namespace RenamerTests
{
    public class FileRenamerTests
    {
		public FileRenamer subject = new FileRenamer();
		char s = Path.DirectorySeparatorChar;

		[Fact]
        public void WhenRenameModeIsUnknown_ItShouldReturnAnEmptyDictionary()
        {
	        var instructions = new RenameInstructions(RenameMode.Unknown, new List<FileInformation>
	        {
		        new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt",".txt", DateTime.UtcNow)
	        });

            var result = subject.Rename(instructions);
            var expected = new Dictionary<string, string >();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenFileInformationListIsEmpty_ItShouldReturnAnEmptyDictionary()
        {
			var instructions = new RenameInstructions(RenameMode.Numerical, new List<FileInformation>());

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>();

			Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenRenamingOneFileNumerical_ItShouldStartAtOne()
        {
	        var instructions = new RenameInstructions(RenameMode.Numerical, new List<FileInformation>
	        {
		        new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt",".txt", DateTime.UtcNow)
	        });

	        var result = subject.Rename(instructions);
	        var expected = new Dictionary<string, string>
	        {
		        { $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"}
	        };

            Assert.Equal(expected, result);
        }

		[Fact]
		public void WhenRenamingMultipleFilesNumerically_ItShouldIncrementEachFilenameByOne()
		{
			var instructions = new RenameInstructions(RenameMode.Numerical, new List<FileInformation>
	        {
		        new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt",".txt", DateTime.UtcNow),
		        new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt",".txt", DateTime.UtcNow)
	        });

	        var result = subject.Rename(instructions);
	        var expected = new Dictionary<string, string>
	        {
		        { $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"},
		        { $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}2.txt"},

	        };

            Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenRenamingMultipleFilesNumerically_ItShouldDoSoBasedOnTheirCreationDateTime()
		{
			var instructions = new RenameInstructions(RenameMode.Numerical, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt",".txt", DateTime.UtcNow.AddDays(1)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt",".txt", DateTime.UtcNow)
			});

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}2.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"},

			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenRenamingOneFileToDate_ItShouldBeRenamedToItsCreationDate()
		{
			var instructions = new RenameInstructions(RenameMode.Date, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt",".txt", new DateTime(2020, 12, 31, 12, 30, 01))
			});

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenRenamingMultipleFilesToDateWithTheSameCreationDate_ItShouldAddNumbers()
		{
			var instructions = new RenameInstructions(RenameMode.Date, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt",".txt", new DateTime(2020, 12, 31, 12, 30, 01)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt",".txt", new DateTime(2020, 12, 31, 12, 30, 01))
			});

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_(1).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_(2).txt"},

			};

			Assert.Equal(expected, result);
		}
	}
}
