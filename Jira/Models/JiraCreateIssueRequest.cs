using System.Text.Json.Serialization;

namespace SlackJiraAutomation.Api.Jira.Models;

public class JiraCreateIssueRequest
{
    [JsonPropertyName("fields")]
    public JiraFields Fields { get; set; } = new();
}

public class JiraFields
{
    [JsonPropertyName("project")]
    public JiraProject Project { get; set; } = new();

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public JiraDescription Description { get; set; } = new();

    [JsonPropertyName("issuetype")]
    public JiraIssueType IssueType { get; set; } = new();
}

public class JiraProject
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;
}

public class JiraIssueType
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Task";
}

public class JiraDescription
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "doc";

    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;

    [JsonPropertyName("content")]
    public List<JiraContent> Content { get; set; } = new();
}

public class JiraContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "paragraph";

    [JsonPropertyName("content")]
    public List<JiraText> Content { get; set; } = new();
}

public class JiraText
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}