using System;

namespace Renamer
{
    public class FileInformation
    {
        public string Path { get; }
        public DateTime CreationDate { get; }

        public FileInformation(string path, DateTime creationDate)
        {
            Path = path;
            CreationDate = creationDate;
        }
    }
}
