using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Xunit;

namespace RenamerTests.Composers
{
	public class DateTimeComposerTests
	{
		public DateTimeComposer subject = new DateTimeComposer();
		private ComposeInstructions Instructions;
		readonly char s = Path.DirectorySeparatorChar;

		[Fact]
		public void WhenRenamingOneFileToDate_ItShouldBeRenamedToItsCreationDate()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Rename(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenRenamingMultipleFilesToDateWithTheSameCreationDate_ItShouldAddNumbers()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Date);
			SetFiles(new List<(string, DateTime)> {
				("fileA", new DateTime(2020, 12, 30, 12, 00, 00)),
				("fileB", new DateTime(2020, 12, 31, 12, 30, 01)),
				("fileC", new DateTime(2020, 12, 31, 12, 30, 01)),
				("fileD", new DateTime(2020, 12, 31, 12, 30, 01)),
				("fileE", new DateTime(2021, 01, 01, 09, 00, 00)),

			});

			var result = subject.Rename(Instructions);
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
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.DateTime);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Rename(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_123001.txt"},
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
