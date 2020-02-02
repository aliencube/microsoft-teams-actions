using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    /// <summary>
    /// This provides interfaces to the <see cref="MessageHandler" /> class.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Gets the JSON serialised message.
        /// </summary>
        string Converted { get; }

        /// <summary>
        /// Gets the incoming webhook URI to Microsoft Teams.
        /// </summary>
        string RequestUri { get; }

        /// <summary>
        /// Builds a message in Actionable Message Format.
        /// </summary>
        /// <param name="options"><see cref="Options" /> instance.</param>
        /// <param name="settings"><see cref="JsonSerializerSettings" /> instance.</param>
        IMessageHandler BuildMessage(Options options, JsonSerializerSettings settings);

        /// <summary>
        /// Sends a message to a Microsoft Teams channel.
        /// </summary>
        /// <param name="client"><see cref="HttpClient" /> instance.</param>
        Task<int> SendMessageAsync(HttpClient client);
    }
}
