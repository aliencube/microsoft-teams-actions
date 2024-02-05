using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using CommandLine;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    /// <summary>
    /// This represents the console app entity.
    /// </summary>
    public static class Program
    {
        static Program() {
            HttpClient = new HttpClient(new RetryHandler(new HttpClientHandler()))
            {
                Timeout = TimeSpan.FromSeconds(100) // Default one, just to be easier to customize
            };
        }

        /// <summary>
        /// Gets or sets the <see cref="IMessageHandler"/> instance.
        /// </summary>
        public static IMessageHandler MessageHandler { get; set; } = new MessageHandler();

        /// <summary>
        /// Gets or sets the <see cref="HttpClient"/> instance.
        /// </summary>
        public static HttpClient HttpClient { get; set; }

        private static JsonSerializerSettings settings { get; } =
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Converters = new List<JsonConverter> { new ActionConverter() },
            };

        /// <summary>
        /// Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments passed.</param>
        public static int Main(string[] args)
        {
            using (var parser = new Parser(with => { with.EnableDashDash = true; with.HelpWriter = Console.Out; }))
            {
                var result = parser.ParseArguments<Options>(args)
                                   .MapResult(options => OnParsed(options), errors => OnNotParsed(errors))
                                   ;

                return result;
            }
        }

        private static int OnParsed(Options options)
        {
            var result = MessageHandler.BuildMessage(options, settings)
                                       .SendMessageAsync(HttpClient)
                                       .Result;

            return result;
        }

        private static int OnNotParsed(IEnumerable<Error> errors)
        {
            return errors.Count();
        }
    }
}
