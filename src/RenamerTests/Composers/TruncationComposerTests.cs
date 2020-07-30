using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Renamer.Dto;
using Xunit;

namespace RenamerTests.Composers
{
	public class TruncationComposerTests
	{
		public TruncationComposer subject = new TruncationComposer();
		private ComposeInstructions Instructions;
		readonly char s = Path.DirectorySeparatorChar;

		[Fact]
		public void WhenTruncationTextIsNotFoundInAnyFileName_ItShouldReturnAnError()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Truncation);
			SetCustomText("fileC");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ "Error", "Custom text to truncate not found in any filename."}
			};

			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhenTruncatingText_ItShouldOnlyModifyFilesContainingTheText()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Truncation);
			SetCustomText("A");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}file.txt"},
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

		private void SetCustomText(string text)
		{
			Instructions.CustomText = text;
		}
	}
}
