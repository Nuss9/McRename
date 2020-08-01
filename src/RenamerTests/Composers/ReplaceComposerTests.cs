using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Renamer.Dto;
using Xunit;

namespace RenamerTests.Composers
{
	public class ReplaceComposerTests
	{
		public ReplaceComposer subject = new ReplaceComposer();
		private ComposeInstructions Instructions;
		readonly char s = Path.DirectorySeparatorChar;

		[Fact]
		public void WhenTReplacingTextIsNotFoundInAnyFileName_ItShouldReturnAnError()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Replace);
			SetCustomText(string.Empty);
			SetTextToReplace("fileC");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ "Error", "Custom text to replace not found in any filename."}
			};

			Assert.Equal(expected, result);
		}


        [Fact]
		public void WhenReplacingText_ItShouldOnlyModifyFilesContainingTheText()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.Replace);
			SetCustomText(string.Empty);
			SetTextToReplace("A");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}file.txt"},
			};

			Assert.Equal(expected, result);
		}

		private void SetComposeAction(ComposeAction action) => Instructions.Action = action;

        private void SetDefaultInstructions() => Instructions = new ComposeInstructions(ComposeMode.Unknown, new List<FileInformation>());

        private void SetComposeMode(ComposeMode2 mode) => Instructions.Mode2 = mode;

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

		private void SetTextToReplace(string text) => Instructions.TextToReplace = text;

		private void SetCustomText(string text) => Instructions.CustomText = text;
    }
}
