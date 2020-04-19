using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Xunit;

namespace RenamerTests
{
    public class FileNameComposerTests
    {
		public FileNameComposer subject = new FileNameComposer();
		private ComposeInstructions Instructions;
        readonly char s = Path.DirectorySeparatorChar;

		[Fact]
        public void WhenComposeModeIsUnknown_ItShouldReturnAnErrorMessage()
        {
			SetDefaultInstructions();

            var result = subject.Rename(Instructions);
            var expected = new Dictionary<string, string> { { "Error message", "Compose mode unknown."} };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenFileInformationListIsEmpty_ItShouldReturnAnErrorMessage()
        {
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Numerical);

			var result = subject.Rename(Instructions);
			var expected = new Dictionary<string, string> { { "Error message", "No files found in selected directory." } };

			Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenRenamingOneFileNumerical_ItShouldStartAtOne()
        {
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now) });

	        var result = subject.Rename(Instructions);
	        var expected = new Dictionary<string, string>
	        {
		        { $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"}
	        };

            Assert.Equal(expected, result);
        }

		[Fact]
		public void WhenRenamingMultipleFilesNumerically_ItShouldIncrementEachFilenameByOne()
		{
			var instructions = new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation>
	        {
		        new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", DateTime.UtcNow),
		        new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", DateTime.UtcNow)
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
			var instructions = new ComposeInstructions(ComposeMode.Numerical, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", DateTime.UtcNow.AddDays(1)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", DateTime.UtcNow)
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
			var instructions = new ComposeInstructions(ComposeMode.Date, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", new DateTime(2020, 12, 31, 12, 30, 01))
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
			var instructions = new ComposeInstructions(ComposeMode.Date, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", new DateTime(2020, 12, 30, 12, 00, 00)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", new DateTime(2020, 12, 31, 12, 30, 01)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileC.txt", new DateTime(2020, 12, 31, 12, 30, 01)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileD.txt", new DateTime(2020, 12, 31, 12, 30, 01)),
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileE.txt", new DateTime(2021, 01, 01, 09, 00, 00))
			});

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201230.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_(1).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileC.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_(2).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileD.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_(3).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileE.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20210101.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenRenamingOneFileToDateTime_ItShouldBeRenamedToItsCreationDateTime()
		{
			var instructions = new ComposeInstructions(ComposeMode.DateTime, new List<FileInformation>
			{
				new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", new DateTime(2020, 12, 31, 12, 30, 01))
			});

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_123001.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void BeforeReturningTheProposedNameChanges_ItShouldValidateAgainstDuplicates()
		{
			var expected = new Dictionary<string, string> { { "Error message", "Aborted renaming due to duplicates in end result." } };

			// How to test this?
		}

        [Fact]
        public void WhenRenamingToCustomText_ItShouldAppendSequenceNumbers()
		{
			var instructions = new ComposeInstructions(
                ComposeMode.CustomText,
                "Holiday_Pictures",
                new List<FileInformation>
			    {
				    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", DateTime.UtcNow),
				    new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", DateTime.UtcNow)
			    });

			var result = subject.Rename(instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}Holiday_Pictures_(1).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}Holiday_Pictures_(2).txt"},
			};

			Assert.Equal(expected, result);
		}

        private void SetDefaultInstructions()
        {
			Instructions = new ComposeInstructions(ComposeMode.Unknown, new List<FileInformation>());
        }

        private void SetComposeMode(ComposeMode mode)
        {
			Instructions.Mode = mode;
        }

		private void SetFiles(List<(string name, DateTime created)> files)
		{
			foreach (var file in files)
			{
				Instructions.Files.Add(
					new FileInformation(
						$"{s}Users{s}JohnDoe{s}Desktop{s}{file.name}.txt",
						file.created
					)
				);
			}
		}
	}
}
