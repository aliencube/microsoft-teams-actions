using System.Linq;
using System.Net.Http;

using Aliencube.GitHubActions.Teams.ConsoleApp;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json;

namespace GitHubActions.Teams.ConsoleApp.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(Program)
                .Should().HaveProperty<IMessageHandler>("MessageHandler")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          ;

            typeof(Program)
                .Should().HaveProperty<HttpClient>("HttpClient")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(Program)
                .Should().HaveMethod("Main", new[] { typeof(string[]) })
                ;
        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        public void Given_Valid_Args_When_Main_Invoked_Then_It_Should_Return_Result(int exitCode, int expected)
        {
            var handler = new Mock<IMessageHandler>();
            handler.Setup(p => p.BuildMessage(It.IsAny<Options>(), It.IsAny<JsonSerializerSettings>())).Returns(handler.Object);
            handler.Setup(p => p.SendMessageAsync(It.IsAny<HttpClient>())).ReturnsAsync(exitCode);

            Program.MessageHandler = handler.Object;

            var args = new[] {
                "--webhook-uri",
                "https://localhosts",
                "--summary",
                "This is the summary",
                "--sections",
                "[ { \"activityTitle\": \"This is activity title\" } ]",
                "--actions",
                "[ { \"@type\": \"OpenUri\", \"name\": \"lorem ipsum\", \"targets\": [ { \"os\": \"default\", \"uri\": \"https://localhost\" } ] } ]"
            }.ToArray();

            var result = Program.Main(args);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_Invalid_Args_When_Main_Invoked_Then_It_Should_Return_Result()
        {
            var handler = new Mock<IMessageHandler>();
            handler.Setup(p => p.BuildMessage(It.IsAny<Options>(), It.IsAny<JsonSerializerSettings>())).Returns(handler.Object);
            handler.Setup(p => p.SendMessageAsync(It.IsAny<HttpClient>())).ReturnsAsync(0);

            Program.MessageHandler = handler.Object;

            var args = new[] {
                "--webhook-uri",
                "https://localhosts",
                "--summary",
                "--sections",
                "[ { \"activityTitle\": \"This is activity title\" } ]",
                "--actions",
                "[ { \"@type\": \"OpenUri\", \"name\": \"lorem ipsum\", \"targets\": [ { \"os\": \"default\", \"uri\": \"https://localhost\" } ] } ]"
            }.ToArray();

            var result = Program.Main(args);

            result.Should().BeGreaterThan(0);
        }
    }
}
