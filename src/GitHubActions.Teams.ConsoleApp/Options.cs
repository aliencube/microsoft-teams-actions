using CommandLine;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    /// <summary>
    /// This represents the parameters entity for the console app.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the incoming webhook URI to a Teams channel.
        /// </summary>
        [Option("webhook-uri", Required = true, HelpText = "Incoming webhook URI to Microsoft Teams")]
        public virtual string WebhookUri { get; set; }

        /// <summary>
        /// Gets or sets the message title.
        /// </summary>
        [Option("title", Required = false, Default = "", HelpText = "Message title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the message summary.
        /// </summary>
        [Option("summary", Required = false, Default = "", HelpText = "Message summary")]
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        [Option("text", Required = false, Default = "", HelpText = "Message text")]
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the message theme colour.
        /// </summary>
        [Option("theme-color", Required = false, Default = "", HelpText = "Message theme color")]
        public virtual string ThemeColor { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array for the message sections.
        /// </summary>
        [Option("sections", Required = false, Default = "", HelpText = "JSON array for message sections")]
        public virtual string Sections { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array for the message actions.
        /// </summary>
        [Option("actions", Required = false, Default = "", HelpText = "JSON array for message actions")]
        public virtual string Actions { get; set; }
    }
}
