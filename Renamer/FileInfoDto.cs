using System;

namespace Renamer
{
    public class FileInfoDto
    {
        public string Path {get;}
        public string Extension {get;}
        public DateTime CreationDateTime {get;}

        public FileInfoDto(string path, string extension, DateTime creationDateTime)
        {
            Path = path;
            Extension = extension;
            CreationDateTime = creationDateTime;
        }
    }
}
