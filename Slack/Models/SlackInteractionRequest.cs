using System.Text.Json.Serialization;

namespace SlackJiraAutomation.Api.Slack.Models;

public class SlackInteractionRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("trigger_id")]
    public string TriggerId { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public SlackInteractionUser User { get; set; } = new();

    [JsonPropertyName("channel")]
    public SlackInteractionChannel? Channel { get; set; }

    [JsonPropertyName("message")]
    public SlackInteractionMessage? Message { get; set; }

    [JsonPropertyName("actions")]
    public List<SlackInteractionAction> Actions { get; set; } = new();

    // Present only for modal submissions
    [JsonPropertyName("view")]
    public SlackInteractionView? View { get; set; }
}

public class SlackInteractionUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class SlackInteractionChannel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class SlackInteractionMessage
{
    [JsonPropertyName("ts")]
    public string Ts { get; set; } = string.Empty;

    [JsonPropertyName("thread_ts")]
    public string? ThreadTs { get; set; }
}

public class SlackInteractionAction
{
    [JsonPropertyName("action_id")]
    public string ActionId { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

public class SlackInteractionView
{
    [JsonPropertyName("callback_id")]
    public string CallbackId { get; set; } = string.Empty;

    [JsonPropertyName("private_metadata")]
    public string PrivateMetadata { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public SlackViewState State { get; set; } = new();
}

public class SlackViewState
{
    [JsonPropertyName("values")]
    public Dictionary<string, Dictionary<string, SlackViewValue>> Values { get; set; }
        = new();
}

public class SlackViewValue
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}