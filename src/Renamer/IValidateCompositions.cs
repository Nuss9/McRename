using System.Collections.Generic;

namespace Renamer
{
    public interface IValidateCompositions
    {
        (bool isValid, string errorMessage) Validate(Dictionary<string, string> composition);
    }
}
