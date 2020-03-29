using System.Collections.Generic;
using System.IO;
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

            var instructions = ConsoleProgram.GetInstructions();

            var renamer = serviceProvider.GetService<IRename>();
			var proposal = renamer.Rename(instructions);

            PathRewriter.Rewrite(proposal);

            ConsoleProgram.Finished();
        }
    }
}
