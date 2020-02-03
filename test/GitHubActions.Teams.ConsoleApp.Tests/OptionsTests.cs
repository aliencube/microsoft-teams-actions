using Aliencube.GitHubActions.Teams.ConsoleApp;

using CommandLine;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitHubActions.Teams.ConsoleApp.Tests
{
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(Options)
                .Should().HaveProperty<string>("WebhookUri")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(Options)
                .Should().HaveProperty<string>("Title")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(Options)
                .Should().HaveProperty<string>("Summary")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(Options)
                .Should().HaveProperty<string>("Text")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(Options)
                .Should().HaveProperty<string>("ThemeColor")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(Options)
                .Should().HaveProperty<string>("Sections")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(Options)
                .Should().HaveProperty<string>("Actions")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Decorators()
        {
            typeof(Options)
                .Should().HaveProperty<string>("WebhookUri")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("webhook-uri") &&
                             p.Required == true)
                    ;

            typeof(Options)
                .Should().HaveProperty<string>("Title")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("title") &&
                             p.Required == false &&
                             p.Default as string == string.Empty)
                    ;

            typeof(Options)
                .Should().HaveProperty<string>("Summary")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("summary") &&
                             p.Required == true)
                    ;

            typeof(Options)
                .Should().HaveProperty<string>("Text")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("text") &&
                             p.Required == false &&
                             p.Default as string == string.Empty)
                    ;

            typeof(Options)
                .Should().HaveProperty<string>("ThemeColor")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("theme-color") &&
                             p.Required == false &&
                             p.Default as string == string.Empty)
                    ;

            typeof(Options)
                .Should().HaveProperty<string>("Sections")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("sections") &&
                             p.Required == false &&
                             p.Default as string == string.Empty)
                    ;

            typeof(Options)
                .Should().HaveProperty<string>("Actions")
                    .Which.Should().BeDecoratedWith<OptionAttribute>(
                        p => p.LongName.Equals("actions") &&
                             p.Required == false &&
                             p.Default as string == string.Empty)
                    ;
        }
    }
}
