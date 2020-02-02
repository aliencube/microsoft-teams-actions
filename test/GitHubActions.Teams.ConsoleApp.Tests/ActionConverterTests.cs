using System;
using System.IO;

using Aliencube.GitHubActions.Teams.ConsoleApp;

using FluentAssertions;

using MessageCardModel;
using MessageCardModel.Actions;

using Newtonsoft.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitHubActions.Teams.ConsoleApp.Tests
{
    [TestClass]
    public class ActionConverterTests
    {
        [TestMethod]
        public void Given_Type_It_Should_Inherit()
        {
            typeof(ActionConverter)
                .Should().BeDerivedFrom<JsonConverter>();
        }

        [TestMethod]
        public void Given_Value_When_WriteJson_Invoked_THen_It_Should_Throw()
        {
            var converter = new ActionConverter();

            Action action = () => converter.WriteJson(null, null, null);

            action.Should().Throw<NotImplementedException>();
        }

        [DataTestMethod]
        [DataRow(typeof(BaseAction), true)]
        [DataRow(typeof(ActionCardAction), false)]
        [DataRow(typeof(HttpPostAction), false)]
        [DataRow(typeof(OpenUriAction), false)]
        public void Given_Type_When_CanConvert_Invoked_It_Should_Return(Type type, bool expected)
        {
            var converter = new ActionConverter();

            var result = converter.CanConvert(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("{ \"@type\": \"ActionCard\" }", typeof(ActionCardAction))]
        [DataRow("{ \"@type\": \"HttpPOST\" }", typeof(HttpPostAction))]
        [DataRow("{ \"@type\": \"OpenUri\" }", typeof(OpenUriAction))]
        public void Given_Json_Value_When_ReadJson_Invoked_Then_It_Should_Return(string json, Type expected)
        {
            var serialiser = new JsonSerializer();
            var result = default(object);

            var converter = new ActionConverter();
            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                result = converter.ReadJson(jsonReader, null, null, serialiser);
            }

            result.Should().BeOfType(expected);
        }
    }
}
