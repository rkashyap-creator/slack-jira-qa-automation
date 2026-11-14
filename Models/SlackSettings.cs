namespace SlackJiraAutomation.Api.Models
{
    public class SlackSettings
    {
        public string BotToken { get; set; } = string.Empty;
        public string AppToken { get; set; } = string.Empty;
        public string SigningSecret { get; set; } = string.Empty;
    }
}