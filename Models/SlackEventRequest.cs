using System.Text.Json.Serialization;

namespace SlackJiraAutomation.Api.Models.Slack;

public class SlackEventRequest
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("team_id")]
    public string? TeamId { get; set; }

    [JsonPropertyName("api_app_id")]
    public string? ApiAppId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("challenge")]
    public string? Challenge { get; set; }

    [JsonPropertyName("event")]
    public SlackEvent? Event { get; set; }
}

public class SlackEvent
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("reaction")]
    public string? Reaction { get; set; }

    [JsonPropertyName("item")]
    public SlackItem? Item { get; set; }

    [JsonPropertyName("event_ts")]
    public string? EventTs { get; set; }
}

public class SlackItem
{
    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    [JsonPropertyName("ts")]
    public string? Ts { get; set; }
}