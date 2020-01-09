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
            Console.WriteLine(options.WebhookUri.StartsWith("h"));

            var card = new MessageCard()
            {
                Title = ParseString(options.Title),
                Summary = ParseString(options.Summary),
                Text = ParseString(options.Text),
                ThemeColor = ParseString(options.ThemeColor),
                Sections = ParseCollection<Section>(options.Sections),
                Actions = ParseCollection<BaseAction>(options.Actions)
            };

            var converted = JsonConvert.SerializeObject(card, settings);
            var message = (string)null;
            var requestUri = options.WebhookUri;

            using (var client = new HttpClient())
            using (var content = new StringContent(converted, Encoding.UTF8, "application/json"))
            using (var response = client.PostAsync(requestUri, content).Result)
            {
                try
                {
                    response.EnsureSuccessStatusCode();

                    message = converted;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            Console.WriteLine($"Message sent: {message}");
        }

        private static string ParseString(string value)
        {
            var parsed = string.IsNullOrWhiteSpace(value)
                         ? null
                         : value;

            return parsed;
        }

        private static List<T> ParseCollection<T>(string value)
        {
            var parsed = string.IsNullOrWhiteSpace(value)
                         ? null
                         : JsonConvert.DeserializeObject<List<T>>(value, settings);

            return parsed;
        }
    }
}
