using System;
using Renamer;
using Xunit;

namespace RenamerTests
{
    public class PathRewriterTests
    {
		public PathRewriter subject = new PathRewriter();

        [Fact]
        public void WhenRenamingToDate_ItShouldFormatToCreationDate()
        {
			string expected = "/Users/Me/20001231.txt";
			string inputPath = "/Users/Me/file.txt";
			DateTime creationDate = new DateTime(2000, 12, 31);

			string result = subject.Execute(inputPath, creationDate);

			Assert.Equal(expected, result);
        }
    }
}
