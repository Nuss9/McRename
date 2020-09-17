using System.Collections.Generic;
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
            textProvider = Substitute.For<IProvideTexts>();
            orchestrator = Substitute.For<IOrchestrate>();
            rewriter = Substitute.For<IRewrite>();

            subject = new Session(textProvider, orchestrator, rewriter);
        }

        [Fact]
        public void WhenExecutingASingleSession_ItShouldCallInOrder()
        {
            textProvider.GetInstructions().Returns(new ComposeInstructions(Renamer.ComposeMode2.Replace, Renamer.ComposeAction.Numerical, new List<FileInformation>()));
            orchestrator.Orchestrate(Arg.Any<ComposeInstructions>()).Returns(new Dictionary<string,string>());
            rewriter.Rewrite(Arg.Any<Dictionary<string, string>>());
            textProvider.AskForBoolean(Arg.Any<string>()).Returns(false);

            subject.Execute();

            textProvider.Received(1).WelcomeMessage();
            textProvider.Received(1).GetInstructions();
            orchestrator.Received(1).Orchestrate(Arg.Any<ComposeInstructions>());
            rewriter.Received(1).Rewrite(Arg.Any<Dictionary<string, string>>());
            textProvider.Received(1).Finished();
        }

        [Fact]
        public void WhenExecutingADoubleSession_ItShouldCallInOrder()
        {
            textProvider.GetInstructions().Returns(new ComposeInstructions(Renamer.ComposeMode2.Replace, Renamer.ComposeAction.Numerical, new List<FileInformation>()));
            orchestrator.Orchestrate(Arg.Any<ComposeInstructions>()).Returns(new Dictionary<string, string>());
            rewriter.Rewrite(Arg.Any<Dictionary<string, string>>());
            textProvider.AskForBoolean(Arg.Any<string>()).Returns(false);

            subject.Execute();

            textProvider.Received(1).WelcomeMessage();
            textProvider.Received(2).GetInstructions();
            orchestrator.Received(2).Orchestrate(Arg.Any<ComposeInstructions>());
            rewriter.Received(2).Rewrite(Arg.Any<Dictionary<string, string>>());
            textProvider.Received(1).Finished();
        }
    }
}
