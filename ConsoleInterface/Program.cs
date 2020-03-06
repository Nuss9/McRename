using Renamer;

namespace ConsoleInterface
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var console = new ConsoleTexts();

            (int mode, string path) dto = console.Execute();

            var renamer = new BatchRenamer();
            renamer.Execute(dto.mode, dto.path);

            ConsoleTexts.Finished();
        }
    }
}
