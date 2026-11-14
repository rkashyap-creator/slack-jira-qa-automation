using SlackJiraAutomation.Api.Jira.Models;

namespace SlackJiraAutomation.Api.Interfaces;

public interface IJiraService
{
    Task<JiraIssueResponse?> CreateIssueAsync(
        string summary,
        string description,
        string issueType = "Task");

    Task<bool> TestConnectionAsync();
}