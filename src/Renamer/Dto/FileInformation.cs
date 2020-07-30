using System;

namespace Renamer.Dto
{
	public class FileInformation
	{
		public string Path { get; }
		public DateTime CreationDateTime { get; }

		public FileInformation(string path, DateTime creationDateTime)
		{
			Path = path;
			CreationDateTime = creationDateTime;
		}
	}
}
