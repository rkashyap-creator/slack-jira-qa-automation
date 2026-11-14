using SlackJiraAutomation.Api.Models.Slack;

namespace SlackJiraAutomation.Api.Interfaces;

public interface ISlackService
{
    Task<bool> TestConnectionAsync();

    Task<bool> VerifyRequestAsync(HttpRequest request);

    Task HandleEventAsync(SlackEventRequest request);
}