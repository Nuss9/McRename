using System;
using System.Collections.Generic;
using System.IO;
using Renamer;
using Renamer.Composers;
using Renamer.Dto;
using Xunit;

namespace RenamerTests.Composers
{
	public class ExtensionComposerTests
	{
		public ExtensionComposer subject = new ExtensionComposer();
		private ComposeInstructions Instructions;
		readonly char s = Path.DirectorySeparatorChar;

		[Fact]
		public void WhenRenamingAnExtension_ItShouldNotModifyTheFilename()
		{
			SetDefaultInstructions();
			SetComposeMode(ComposeMode.Extension);
			SetCustomText("png");
			SetFiles(new List<(string, DateTime)> { ("fileA", DateTime.Now) });

			var result = subject.Compose(Instructions);
			var expected = new Dictionary<string, string>
			{
				{ $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.txt", $"{s}Users{s}JohnDoe{s}Desktop{s}fileA.png"}
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
