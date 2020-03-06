using System;
using Renamer;

namespace ConsoleInterface
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            (int mode, string path) dto = ConsoleTexts.Execute();

            var renamer = new BatchRenamer();
            renamer.Execute(dto.mode, dto.path);

            ConsoleTexts.Finished();
        }
    }
}
