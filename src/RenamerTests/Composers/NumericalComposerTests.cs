using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
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
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Rename(Instructions);
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
			SetComposeMode(ComposeMode.Numerical);
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.UtcNow.AddDays(1)), ("fileB", DateTime.Now) });

			var result = subject.Rename(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}2.txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}1.txt"},

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
