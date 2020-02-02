using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MessageCardModel;

using Newtonsoft.Json;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    /// <summary>
    /// This represents the console app entity.
    /// </summary>
    public class MessageHandler : IMessageHandler
    {
        /// <inheritdoc />
        public virtual string Converted { get; private set; }

        /// <inheritdoc />
        public virtual string RequestUri { get; private set; }

        /// <inheritdoc />
        public IMessageHandler BuildMessage(Options options, JsonSerializerSettings settings)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var card = new MessageCard()
            {
                Title = ParseString(options.Title),
                Summary = ParseString(options.Summary),
                Text = ParseString(options.Text),
                ThemeColor = ParseString(options.ThemeColor),
                Sections = ParseCollection<Section>(options.Sections, settings),
                Actions = ParseCollection<BaseAction>(options.Actions, settings)
            };

            this.Converted = JsonConvert.SerializeObject(card, settings);
            this.RequestUri = options.WebhookUri;

            return this;
        }

        /// <inheritdoc />
        public async Task<int> SendMessageAsync(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrWhiteSpace(this.RequestUri))
            {
                throw new InvalidOperationException("Webhook URI not ready");
            }

            var message = default(string);
            var exitCode = default(int);

            using (var content = new StringContent(this.Converted, Encoding.UTF8, "application/json"))
            using (var response = await client.PostAsync(this.RequestUri, content).ConfigureAwait(false))
            {
                try
                {
                    response.EnsureSuccessStatusCode();

                    message = $"Message sent: {this.Converted}";
                    exitCode = 0;
                }
                catch (HttpRequestException ex)
                {
                    message = $"Error: {ex.Message}";
                    exitCode = (int) response.StatusCode;
                }
            }

            Console.WriteLine(message);

            return exitCode;
        }

        private static string ParseString(string value)
        {
            var parsed = string.IsNullOrWhiteSpace(value)
                         ? null
                         : value;

            return parsed;
        }

        private static List<T> ParseCollection<T>(string value, JsonSerializerSettings settings)
        {
            var parsed = string.IsNullOrWhiteSpace(value)
                         ? null
                         : JsonConvert.DeserializeObject<List<T>>(value, settings);

            return parsed;
        }
    }
}
