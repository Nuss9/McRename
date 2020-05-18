using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer.Composers
{
    public class DateTimeComposer : ICompose
    {
        private readonly char Separator;

        public DateTimeComposer()
        {
            Separator = Path.DirectorySeparatorChar;
        }

        public Dictionary<string, string> Rename(ComposeInstructions instructions)
        {
            int duplicateCounter = 1;
            var proposal = new Dictionary<string, string>();

            foreach (var file in instructions.Files)
            {
                var path = file.Path;
                var directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);

                string dateFormat;
                if (instructions.Mode == ComposeMode.Date)
                {
                    dateFormat = "yyyyMMdd";
                }
                else
                {
                    dateFormat = "yyyyMMdd_HHmmss";
                }
                var creationDate = file.CreationDateTime.ToString(dateFormat);

                if (proposal.Any())
                {
                    if (proposal.Values.Last().EndsWith($"{creationDate}{extension}"))
                    {
                        string lastKey = proposal.Keys.Last();
                        proposal.Remove(lastKey);
                        proposal.Add(lastKey, $"{directory}{Separator}{creationDate}_({duplicateCounter}){extension}");

                        duplicateCounter++;
                        creationDate += $"_({duplicateCounter})";
                        duplicateCounter++;
                    }
                    else if (proposal.Values.Last().Contains(creationDate))
                    {
                        creationDate += $"_({duplicateCounter})";
                        duplicateCounter++;
                    }
                    else
                    {
                        duplicateCounter = 1;
                    }
                }

                string newPath = $"{directory}{Separator}{creationDate}{extension}";

                proposal.Add(path, newPath);
            }

            return proposal;
        }
    }
}
