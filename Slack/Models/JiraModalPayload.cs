using System.Text.Json.Serialization;

namespace SlackJiraAutomation.Api.Slack.Models;

public class JiraModalPayload
{
    [JsonPropertyName("trigger_id")]
    public string TriggerId { get; set; } = string.Empty;

    [JsonPropertyName("view")]
    public JiraView View { get; set; } = new();
}

public class JiraView
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "modal";

    [JsonPropertyName("callback_id")]
    public string CallbackId { get; set; } = "create_jira";

    // VERY IMPORTANT
    // We will store Slack thread information here.
    [JsonPropertyName("private_metadata")]
    public string PrivateMetadata { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public JiraPlainText Title { get; set; } = new();

    [JsonPropertyName("submit")]
    public JiraPlainText Submit { get; set; } = new();

    [JsonPropertyName("close")]
    public JiraPlainText Close { get; set; } = new();

    [JsonPropertyName("blocks")]
    public List<JiraBlock> Blocks { get; set; } = new();
}

public class JiraBlock
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("block_id")]
    public string BlockId { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public JiraPlainText Label { get; set; } = new();

    [JsonPropertyName("element")]
    public JiraInputElement Element { get; set; } = new();
}

public class JiraInputElement
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "plain_text_input";

    [JsonPropertyName("action_id")]
    public string ActionId { get; set; } = string.Empty;

    [JsonPropertyName("multiline")]
    public bool Multiline { get; set; }

    [JsonPropertyName("placeholder")]
    public JiraPlainText Placeholder { get; set; } = new();
}

public class JiraPlainText
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "plain_text";

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}