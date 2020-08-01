using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Renamer.Dto;
using Xunit;

namespace RenamerTests.Composers
{
	public class TextComposerTests
	{
		public TextComposer subject = new TextComposer();
		private ComposeInstructions Instructions;
		readonly char s = Path.DirectorySeparatorChar;

		[Fact]
		public void WhenRenamingToCustomText_ItShouldAppendSequenceNumbers()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.CustomText);
			SetCustomText("Holiday_Pictures");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now), ("fileB", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}Holiday_Pictures_(1).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}Holiday_Pictures_(2).txt"},
			};

			Assert.Equal(expected, result);
		}

        [Fact]
		public void WhenRenamingToCustomText_ItShouldNotModifyExtensions()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode2.Replace);
			SetComposeAction(ComposeAction.CustomText);
			SetCustomText("Holiday_Pictures");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now) });
			Instructions.Files.Add(new FileInformation($"{s}Users{s}JohnDoe{s}Desktop{s}fileB.png", DateTime.Now));

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}Holiday_Pictures_(1).txt"},
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileB.png", $"{s}Users{s}JohnDoe{s}Desktop{s}Holiday_Pictures_(2).png"},
			};

			Assert.Equal(expected, result);
		}

        private void SetDefaultInstructions() => Instructions = new ComposeInstructions(ComposeMode2.Unknown, ComposeAction.CustomText, new List<FileInformation>());

        private void SetComposeMode(ComposeMode2 mode) => Instructions.Mode2 = mode;

        private void SetComposeAction(ComposeAction action) => Instructions.Action = action;

        private void SetCustomText(string text) => Instructions.CustomText = text;

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
