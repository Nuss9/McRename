using System.Collections.Generic;
using Renamer;
using Xunit;

namespace RenamerTests
{
    public class CompositionValidatorTests
    {
        [Fact]
        public void WhenCompositionContainsDuplicateOutputFiles_ItShouldReturnAnError()
        {
            var composition = new Dictionary<string, string>
			{
				{ $"/Users/JohnDoe/Desktop/filea.txt", $"/Users/JohnDoe/Desktop/fileA.png"},
				{ $"/Users/JohnDoe/Desktop/fileb.txt", $"/Users/JohnDoe/Desktop/fileA.png"},
				{ $"/Users/JohnDoe/Desktop/filec.txt", $"/Users/JohnDoe/Desktop/fileC.png"},
			};

            var validator = new CompositionValidator();
            var result = validator.Validate(composition);

            Assert.False(result.isValid);
            Assert.NotEmpty(result.errorMessage);
        }
    }
}
