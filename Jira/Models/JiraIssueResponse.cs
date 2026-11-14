namespace SlackJiraAutomation.Api.Jira.Models;

public class JiraIssueResponse
{
    public string Id { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public string Self { get; set; } = string.Empty;

    public string BrowseUrl { get; set; } = string.Empty;
}