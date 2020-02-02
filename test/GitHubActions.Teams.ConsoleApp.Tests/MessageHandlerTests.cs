using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Aliencube.GitHubActions.Teams.ConsoleApp;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WorldDomination.Net.Http;

namespace GitHubActions.Teams.ConsoleApp.Tests
{
    [TestClass]
    public class MessageHandlerTests
    {
        private static JsonSerializerSettings settings { get; } =
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Converters = new List<JsonConverter> { new ActionConverter() },
            };

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(MessageHandler)
                .Should().HaveProperty<string>("Converted")
                    .Which.Should().BeVirtual()
                    .And.BeReadable()
                    .And.BeWritable()
                    .And.BeVirtual()
                    ;

            typeof(MessageHandler)
                .Should().HaveProperty<string>("RequestUri")
                    .Which.Should().BeVirtual()
                    .And.BeReadable()
                    .And.BeWritable()
                    .And.BeVirtual()
                    ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(MessageHandler)
                .Should().HaveMethod("BuildMessage", new[] { typeof(Options), typeof(JsonSerializerSettings) })
                ;

            typeof(MessageHandler)
                .Should().HaveMethod("SendMessageAsync", new List<Type>() { typeof(HttpClient) } )
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(MessageHandler)
                .Should().Implement<IMessageHandler>()
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_BuildMessage_Invoked_Then_It_Should_Throw_Exception()
        {
            var options = new Mock<Options>();
            var handler = new MessageHandler();

            Action action = () => handler.BuildMessage(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => handler.BuildMessage(options.Object, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_BuildMessage_Invoked_Then_It_Should_Not_Exist()
        {
            var options = new Mock<Options>();
            options.SetupGet(p => p.Title).Returns("hello world");

            var handler = new MessageHandler();

            var result = handler.BuildMessage(options.Object, settings);

            var deserialised = JObject.Parse(handler.Converted);

            deserialised.Value<string>("text").Should().BeNull();
        }

        [DataTestMethod]
        [DataRow("hello world", "hello world")]
        public void Given_Title_When_BuildMessage_Invoked_Then_It_Should_Return_Result(string input, string expected)
        {
            var options = new Mock<Options>();
            options.SetupGet(p => p.Title).Returns(input);

            var handler = new MessageHandler();

            var result = handler.BuildMessage(options.Object, settings);

            var deserialised = JObject.Parse(handler.Converted);

            deserialised.Value<string>("title").Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("[ { \"activityImage\": \"https://localhost/image.png\", \"activityTitle\": \"lorem\",  \"activitySubtitle\": \"ipsum\", \"activityText\": \"hello world\" } ]")]
        public void Given_Sections_When_BuildMessage_Invoked_Then_It_Should_Return_Result(string sections)
        {
            var options = new Mock<Options>();
            options.SetupGet(p => p.Sections).Returns(sections);

            var handler = new MessageHandler();

            var result = handler.BuildMessage(options.Object, settings);

            var deserialised = JObject.Parse(handler.Converted);

            deserialised["sections"].AsJEnumerable().Should().HaveCount(1);
        }

        [DataTestMethod]
        [DataRow("[ { \"@type\": \"OpenUri\", \"name\": \"lorem ipsum\", \"targets\": [ { \"os\": \"default\", \"uri\": \"https://localhost\" } ] } ]")]
        public void Given_Actions_When_BuildMessage_Invoked_Then_It_Should_Return_Result(string actions)
        {
            var options = new Mock<Options>();
            options.SetupGet(p => p.Actions).Returns(actions);

            var handler = new MessageHandler();

            var result = handler.BuildMessage(options.Object, settings);

            var deserialised = JObject.Parse(handler.Converted);

            deserialised["potentialAction"].AsJEnumerable().Should().HaveCount(1);
        }

        [TestMethod]
        public void Given_Null_Parameters_When_SendMessageAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var handler = new MessageHandler();

            Func<Task> func = async () => await handler.SendMessageAsync(null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_RequestUri_Property_When_SendMessageAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var options = new Mock<Options>();

            var handler = new MessageHandler();
            handler.BuildMessage(options.Object, settings);

            var httpClient = new HttpClient();

            Func<Task> func = async () => await handler.SendMessageAsync(httpClient).ConfigureAwait(false);
            func.Should().Throw<InvalidOperationException>()
                .Which.Message.Should().BeEquivalentTo("Webhook URI not ready");
        }

        [DataTestMethod]
        [DataRow("lorem ipsum", HttpStatusCode.OK, 0)]
        [DataRow("lorem ipsum", HttpStatusCode.Created, 0)]
        [DataRow("lorem ipsum", HttpStatusCode.Accepted, 0)]
        [DataRow("lorem ipsum", HttpStatusCode.Found, (int)HttpStatusCode.Found)]
        [DataRow("lorem ipsum", HttpStatusCode.BadRequest, (int)HttpStatusCode.BadRequest)]
        [DataRow("lorem ipsum", HttpStatusCode.InternalServerError, (int)HttpStatusCode.InternalServerError)]
        public async Task Given_HttpClient_When_SendMessageAsync_Invoked_Then_It_Should_Return_Result(string message, HttpStatusCode statusCode, int expected)
        {
            var options = new Mock<Options>();
            options.SetupGet(p => p.Title).Returns("hello world");
            options.SetupGet(p => p.WebhookUri).Returns("https://localhost");

            var content = new StringContent(message);
            var response = new HttpResponseMessage(statusCode) { Content = content };
            var messageOptions = new HttpMessageOptions() { HttpResponseMessage = response };
            var httpHandler = new FakeHttpMessageHandler(messageOptions);
            var httpClient = new HttpClient(httpHandler);

            var handler = new MessageHandler();

            handler.BuildMessage(options.Object, settings);

            var result = await handler.SendMessageAsync(httpClient).ConfigureAwait(false);

            result.Should().Be(expected);
        }
    }
}
