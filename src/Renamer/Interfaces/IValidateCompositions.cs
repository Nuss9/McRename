using System.Collections.Generic;

namespace Renamer.Interfaces
{
    public interface IValidateCompositions
    {
        (bool isValid, string errorMessage) Validate(Dictionary<string, string> composition);
    }
}
