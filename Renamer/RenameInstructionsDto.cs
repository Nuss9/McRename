using System.Collections.Generic;

namespace Renamer
{
    public class RenameInstructionsDto
    {
        public int Mode {get;}
        public List<FileInfoDto> Files {get;}

        public RenameInstructionsDto(int mode, List<FileInfoDto> files)
        {
            Mode = mode;
            Files = files;
        }
    }
}