using Microsoft.Extensions.DependencyInjection;
using Renamer;

namespace ConsoleInterface
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IRename, FileRenamer>()
                .BuildServiceProvider();

            //(int mode, string path) dto = ConsoleTexts.Execute();
            //RenameInstructionsDto dto = ConsoleProgram.GetInstructions();
            var result = ConsoleProgram.GetInstructions();

            var renamer = serviceProvider.GetService<IRename>();

            //renamer.Execute(dto.mode, dto.path);

            ConsoleTexts.Finished();
        }
    }
}
