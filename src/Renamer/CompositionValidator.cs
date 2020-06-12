
using System;
using System.Collections.Generic;
using System.Linq;
using Renamer.Interfaces;

namespace Renamer
{
    public class CompositionValidator : IValidateCompositions
    {
        public (bool isValid, string errorMessage) Validate(Dictionary<string, string> composition)
        {
            if (composition.Values.Distinct().Count() == composition.Values.Count())
            {
                return (true, string.Empty);
            }

            return (false, "Duplicate filenames generated.");
        }
    }
}
