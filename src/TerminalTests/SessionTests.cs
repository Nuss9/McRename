using System;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using Renamer.Dto;
using Renamer.Interfaces;
using Terminal;
using Terminal.Interfaces;
using Xunit;

namespace TerminalTests
{
    public class SessionTests
    {
        private readonly Session subject;
        private readonly IProvideTexts textProvider;
        private readonly IOrchestrate orchestrator;
        private readonly IRewrite rewriter;

        public SessionTests()
        {
            subject = new Session();
            textProvider = Substitute.For<IProvideTexts>();
            orchestrator = Substitute.For<IOrchestrate>();
            rewriter = Substitute.For<IRewrite>();
        }

        [Fact]
        public void Test1()
        {
            textProvider.GetInstructions().Returns(new ComposeInstructions(Renamer.ComposeMode2.Replace, Renamer.ComposeAction.Numerical, new List<FileInformation>()));
            orchestrator.Orchestrate(Arg.Any<ComposeInstructions>()).Returns(new Dictionary<string,string>());
            rewriter.Rewrite(Arg.Any<Dictionary<string, string>>());
            textProvider.AskForBoolean(Arg.Any<string>()).Returns(false);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                using (StringReader sr = new StringReader(string.Format("pics{0}1{0}Y{0}n{0}", '\n')))
                {
                    Console.SetIn(sr);

                    subject.Execute();

                    textProvider.Received(1).WelcomeMessage();
                    textProvider.Received(1).GetInstructions();
                    orchestrator.Received(1).Orchestrate(Arg.Any<ComposeInstructions>());
                    rewriter.Received(1).Rewrite(Arg.Any<Dictionary<string, string>>());
                    textProvider.Received(1).Finished();

                    //Everytime no matching calls. Why?
                }
            }
        }
    }
}
