using Microsoft.AspNetCore.Mvc;
using SlackJiraAutomation.Api.Interfaces;

namespace SlackJiraAutomation.Api.Controllers;

[ApiController]
[Route("api/jira")]
public class JiraController : ControllerBase
{
    private readonly IJiraService _jiraService;

    public JiraController(IJiraService jiraService)
    {
        _jiraService = jiraService;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var connected = await _jiraService.TestConnectionAsync();

        return Ok(new
        {
            Connected = connected
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
        var issue = await _jiraService.CreateIssueAsync(
            "Slack Jira Automation Test",
            "This issue was created from the Slack Jira Automation API.",
            "Task");

        if (issue == null)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Failed to create Jira issue."
            });
        }

        return Ok(new
        {
            Success = true,
            IssueKey = issue.Key,
            IssueId = issue.Id,
            IssueUrl = issue.BrowseUrl
        });
    }
}