using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Renamer.Dto;
using Renamer.Exceptions;
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
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
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
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> {
				("fileA", new DateTime(2020, 12, 30, 12, 00, 00)),
				("fileB", new DateTime(2020, 12, 31, 12, 30, 01)),
				("fileC", new DateTime(2020, 12, 31, 12, 30, 01)),
				("fileD", new DateTime(2020, 12, 31, 12, 30, 01)),
				("fileE", new DateTime(2021, 01, 01, 09, 00, 00)),

			});

			var result = subject.Compose(Instructions);
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
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.DateTime);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231_123001.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenPrependingFileNameWithDate_ItShouldAddCreationDateAtTheBeginning()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Prepend);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}20201231fileA.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenInsertingFileNameWithDate_ItShouldInsertCreationDateAtTheRightPlace()
		{
			SetDefaultInstructions();
			Instructions.InsertPosition = 3;
			SetComposeMode(ComposeMode2.Insert);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fil20201231eA.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenInsertingFileNameWithDateOutOfRange_ItShouldInsertCreationDateAtTheEndOfBaseName()
		{
			SetDefaultInstructions();
			Instructions.InsertPosition = 300;
			SetComposeMode(ComposeMode2.Insert);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fileA20201231.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenAppendingFileNameWithDate_ItShouldAppendCreationDate()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Append);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fileA20201231.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenComposingWithInvalidComposeMode_ItShouldThrow()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Unknown);
			SetComposeAction(ComposeAction.Date);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var ex = Assert.Throws<UnknownComposeModeException>(() => subject.Compose(Instructions));
			Assert.Equal("Invalid mode.", ex.Message);
		}

		private void SetComposeAction(ComposeAction action) => Instructions.Action = action;

		private void SetDefaultInstructions() => Instructions = new ComposeInstructions(ComposeMode2.Replace, ComposeAction.DateTime, new List<FileInformation>());

		private void SetComposeMode(ComposeMode2 mode2) => Instructions.Mode2 = mode2;

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
