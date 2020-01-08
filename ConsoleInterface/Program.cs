using Renamer;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleTexts console = new ConsoleTexts();

            (int mode, string path) dto = console.Execute();

            BatchRenamer renamer = new BatchRenamer();
            renamer.Execute(dto.mode, dto.path);

            ConsoleTexts.Finished();
        }
    }
}
