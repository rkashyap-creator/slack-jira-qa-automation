using SlackJiraAutomation.Api.Interfaces;
using SlackJiraAutomation.Api.Slack.Builders;
using SlackJiraAutomation.Api.Slack.Clients;
using SlackJiraAutomation.Api.Slack.Models;

namespace SlackJiraAutomation.Api.Slack.Handlers;

public class SlackInteractionHandler
{
    private readonly SlackApiClient _slackApiClient;
    private readonly IJiraService _jiraService;

    public SlackInteractionHandler(
        SlackApiClient slackApiClient,
        IJiraService jiraService)
    {
        _slackApiClient = slackApiClient;
        _jiraService = jiraService;
    }

    public async Task HandleAsync(
        SlackInteractionRequest request)
    {
        if (request == null)
            return;

        //--------------------------------------------------
        // BUTTON CLICK
        //--------------------------------------------------
        if (request.Type == "block_actions")
        {
            if (request.Actions == null ||
                request.Actions.Count == 0)
                return;

            var action = request.Actions.First();

            if (action.ActionId != "log_issue")
                return;

            var channelId =
                request.Channel?.Id ?? string.Empty;

            var threadTs =
                request.Message?.ThreadTs ??
                request.Message?.Ts ??
                string.Empty;

            var messageTs =
                request.Message?.Ts ??
                string.Empty;

            Console.WriteLine("==================================");
            Console.WriteLine("Slack Button Clicked");
            Console.WriteLine($"Channel   : {channelId}");
            Console.WriteLine($"Thread TS : {threadTs}");
            Console.WriteLine($"Message TS: {messageTs}");
            Console.WriteLine("==================================");

            var modal =
                JiraModalBuilder.Build(
                    request.TriggerId,
                    channelId,
                    threadTs,
                    messageTs);

            var opened =
                await _slackApiClient.OpenModalAsync(
                    modal);

            Console.WriteLine(opened
                ? "Jira modal opened successfully."
                : "Failed to open Jira modal.");

            return;
        }

        //--------------------------------------------------
        // MODAL SUBMIT
        //--------------------------------------------------
        if (request.Type == "view_submission")
        {
            Console.WriteLine("==================================");
            Console.WriteLine("Jira Modal Submitted");
            Console.WriteLine("==================================");

            if (request.View == null)
                return;

            var values =
                request.View.State.Values;

            string summary = string.Empty;
            string description = string.Empty;

            if (values.TryGetValue(
                "summary",
                out var summaryBlock))
            {
                if (summaryBlock.TryGetValue(
                    "summary",
                    out var summaryValue))
                {
                    summary =
                        summaryValue.Value ?? "";
                }
            }

            if (values.TryGetValue(
                "description",
                out var descriptionBlock))
            {
                if (descriptionBlock.TryGetValue(
                    "description",
                    out var descriptionValue))
                {
                    description =
                        descriptionValue.Value ?? "";
                }
            }

            Console.WriteLine($"Summary    : {summary}");
            Console.WriteLine($"Description: {description}");

            var issue =
                await _jiraService.CreateIssueAsync(
                    summary,
                    description);

            var metadata =
                request.View.PrivateMetadata.Split('|');

            if (metadata.Length < 2)
            {
                Console.WriteLine(
                    "Private metadata missing.");

                return;
            }

            var channelId = metadata[0];
            var threadTs = metadata[1];

            if (issue != null)
            {
                Console.WriteLine(
                    $"Jira Created : {issue.Key}");

                var payload =
                    SlackBlockBuilder.BuildJiraCreatedMessage(
                        channelId,
                        threadTs,
                        issue.Key,
                        issue.BrowseUrl);

                var posted =
                    await _slackApiClient.PostThreadMessageAsync(
                        payload);

                Console.WriteLine(posted
                    ? "Slack thread updated."
                    : "Failed to update Slack thread.");
            }
            else
            {
                Console.WriteLine(
                    "Failed to create Jira.");

                var payload =
                    SlackBlockBuilder.BuildJiraFailedMessage(
                        channelId,
                        threadTs);

                await _slackApiClient.PostThreadMessageAsync(
                    payload);
            }

            return;
        }

        //--------------------------------------------------
        // UNKNOWN
        //--------------------------------------------------
        Console.WriteLine(
            $"Unknown Interaction : {request.Type}");
    }
}