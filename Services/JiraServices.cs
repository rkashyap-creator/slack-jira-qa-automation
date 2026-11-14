using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SlackJiraAutomation.Api.Interfaces;
using SlackJiraAutomation.Api.Jira.Models;
using SlackJiraAutomation.Api.Models;

namespace SlackJiraAutomation.Api.Jira.Services;

public class JiraService : IJiraService
{
    private readonly HttpClient _httpClient;
    private readonly JiraSettings _settings;

    public JiraService(
        HttpClient httpClient,
        IOptions<JiraSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);

        var auth = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                $"{_settings.Email}:{_settings.ApiToken}"));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", auth);

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> TestConnectionAsync()
    {
        var response =
            await _httpClient.GetAsync("/rest/api/3/myself");

        return response.IsSuccessStatusCode;
    }

    public async Task<JiraIssueResponse?> CreateIssueAsync(
        string summary,
        string description,
        string issueType = "Task")
    {
        var request = new JiraCreateIssueRequest
        {
            Fields = new JiraFields
            {
                Project = new JiraProject
                {
                    Key = _settings.ProjectKey
                },

                Summary = summary,

                IssueType = new JiraIssueType
                {
                    Name = issueType
                },

                Description = new JiraDescription
                {
                    Content =
                    {
                        new JiraContent
                        {
                            Content =
                            {
                                new JiraText
                                {
                                    Text = description
                                }
                            }
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(request);

        var response = await _httpClient.PostAsync(
            "/rest/api/3/issue",
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json"));

        if (!response.IsSuccessStatusCode)
            return null;

        var responseJson = await response.Content.ReadAsStringAsync();

        var issue = JsonSerializer.Deserialize<JiraIssueResponse>(
            responseJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (issue != null)
        {
            issue.BrowseUrl =
                $"{_settings.BaseUrl}/browse/{issue.Key}";
        }

        return issue;
    }
}