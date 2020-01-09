using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using CommandLine;

using MessageCardModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    /// <summary>
    /// This represents the console app entity.
    /// </summary>
    public static class Program
    {
        private static JsonSerializerSettings settings { get; } =
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

        /// <summary>
        /// Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments passed.</param>
        public static void Main(string[] args)
        {
            using (var parser = new Parser(with => { with.EnableDashDash = true; with.HelpWriter = Console.Out; }))
            {
                parser.ParseArguments<Options>(args)
                      .WithParsed<Options>(options => Process(options));
            }
        }

        private static void Process(Options options)
        {
            var card = new MessageCard()
            {
                Title = options.Title,
                Summary = options.Summary,
                Text = options.Text,
                ThemeColor = options.ThemeColor,
                Sections = ParseSections(options.Sections),
                Actions = ParseActions(options.Actions)
            };

            var converted = card.ToJson();

            using (var client = new HttpClient())
            {
                var requestUri = options.WebhookUri;
                var content = new StringContent(converted, Encoding.UTF8, "application/json");

                var response = client.PostAsync(requestUri, content).Result;
            }
        }

        private static List<Section> ParseSections(string sections)
        {
            var parsed = string.IsNullOrWhiteSpace(sections)
                         ? null
                         : JsonConvert.DeserializeObject<List<Section>>(sections, settings);

            return parsed;
        }

        private static List<BaseAction> ParseActions(string actions)
        {
            var parsed = string.IsNullOrWhiteSpace(actions)
                         ? null
                         : JsonConvert.DeserializeObject<List<BaseAction>>(actions, settings);

            return parsed;
        }
    }
}
