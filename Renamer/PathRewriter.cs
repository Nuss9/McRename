using System;
using System.IO;

namespace Renamer
{
    public class PathRewriter
    {
		public string Execute(string path, DateTime fileCreationDateTime)
		{
			if(IsDirectory(path)) { return null; }

			string creationDateTime = fileCreationDateTime.ToString("yyyyMMdd");
			string creationDate = creationDateTime.Substring(0,8);
			string directoryPath = Path.GetDirectoryName(path);
			string extension = Path.GetExtension(path);
			string result = directoryPath + "/" + creationDate + extension;

			return result.Replace("\\", "/");
		}

		private static bool IsDirectory(string path)
		{
			return !Path.HasExtension(path);
		}
	}
}
