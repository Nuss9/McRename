using System.Collections.Generic;
using Renamer;
using Xunit;

namespace RenamerTests
{
    public class FileRenamerTests
    {
		public FileRenamer subject = new FileRenamer();

        [Fact]
        public void WhenRenameModeIsUnknown_ItShouldReturnAnEmptyDictionary()
        {
	        var instructions = new RenameInstructions(RenameMode.Unknown, new List<FileInformation>());
			
            var result = subject.Execute(instructions);
            var expected = new Dictionary<string, string>();
            
            Assert.Equal(expected, result);
        }
    }
}
