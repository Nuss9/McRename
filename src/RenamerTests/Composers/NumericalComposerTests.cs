using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Renamer.Dto;
using Xunit;

namespace RenamerTests.Composers
{
	public class NumericalComposerTests
	{
		public NumericalComposer subject = new NumericalComposer();
		private ComposeInstructions Instructions;
		readonly char s = Path.DirectorySeparatorChar;

		[Fact]
		public void WhenRenamingOneFileNumerical_ItShouldStartAtOne()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"}
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenRenamingMultipleFilesNumerically_ItShouldIncrementEachFilenameByOne()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
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
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.UtcNow.AddDays(1)), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}2.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenPrependingNumerically_ItShouldAddIncrementingNumbersToBaseName()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Prepend);
			SetComposeAction(ComposeAction.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.UtcNow.AddDays(1)), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}2fileA.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1fileB.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenInsertingNumerically_ItShouldInsertIncrementingNumberIntoBaseName()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Insert);
			SetComposeAction(ComposeAction.Numerical);
			Instructions.InsertPosition = 3;
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.UtcNow.AddDays(1)), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fil2eA.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fil1eB.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenInsertingNumericallyOutOfRange_ItShouldInsertTheNumberAtTheEndOfBaseName()
		{
			SetDefaultInstructions();
			Instructions.InsertPosition = 300;
			SetComposeMode(ComposeMode2.Insert);
			SetComposeAction(ComposeAction.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", new DateTime(2020, 12, 31, 12, 30, 01)) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fileA1.txt"},
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenAppendingNumerically_ItShouldAppendIncrementingNumberToBaseName()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Append);
			SetComposeAction(ComposeAction.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.UtcNow.AddDays(1)), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fileA2.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fileB1.txt"},
			};

			Assert.Equal(expected, result);
		}

		private void SetDefaultInstructions() => Instructions = new ComposeInstructions(ComposeMode.Unknown, new List<FileInformation>());

		private void SetComposeMode(ComposeMode2 mode) => Instructions.Mode2 = mode;

		private void SetComposeAction(ComposeAction action) => Instructions.Action = action;

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
