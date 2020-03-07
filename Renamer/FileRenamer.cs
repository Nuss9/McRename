using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Renamer
{
    public class FileRenamer : IRename
    {
        public void Execute(int mode, string path)
        {
            List<string> inputFilePaths = Directory.GetFiles(path).ToList();

            Directory.CreateDirectory(path + "_Renamed");

            foreach(string filePath in inputFilePaths) {
                string newName = GetNewName(mode, filePath);
                string extension = Path.GetExtension(filePath);
                int iteration = 1;
                while(File.Exists(path + "_Renamed/" + newName + extension)) {
                    iteration++;
                    if(mode == 1) {
                        newName = newName.Substring(0, 15) + $"({iteration})";
                    }
                    else {
                        newName = newName.Substring(0, 8) + $"({iteration})";
                    }
                }

                File.Copy(filePath, path + "_Renamed/" + newName + extension);
            }
        }

        private static string GetNewName(int mode, string filePath)
        {
            switch(mode)
            {
                case 1:
                    return File.GetCreationTimeUtc(filePath).ToString("yyyyMMdd_HHmmss");
                case 2:
                    return File.GetCreationTimeUtc(filePath).ToString("yyyyMMdd");
                default:
                    throw new Exception("Invalid mode chosen.");
            }
        }
    }
}
