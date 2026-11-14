using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SlackNet;
using SlackJiraAutomation.Api.Helpers;
using SlackJiraAutomation.Api.Interfaces;
using SlackJiraAutomation.Api.Models;
using SlackJiraAutomation.Api.Models.Slack;
using SlackJiraAutomation.Api.Slack.Builders;

namespace SlackJiraAutomation.Api.Services;

public class SlackService : ISlackService
{
    private readonly SlackSettings _settings;
    private readonly SlackJiraAutomation.Api.Slack.Clients.SlackApiClient _slackApiClient;

    public SlackService(
        IOptions<SlackSettings> options,
        SlackJiraAutomation.Api.Slack.Clients.SlackApiClient slackApiClient)
    {
        _settings = options.Value;
        _slackApiClient = slackApiClient;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var api = new SlackServiceBuilder()
                .UseApiToken(_settings.BotToken)
                .GetApiClient();

            var response = await api.Auth.Test();

            return !string.IsNullOrWhiteSpace(response.UserId);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> VerifyRequestAsync(HttpRequest request)
    {
        try
        {
            if (!request.Headers.TryGetValue("X-Slack-Signature", out var slackSignature))
                return false;

            if (!request.Headers.TryGetValue("X-Slack-Request-Timestamp", out var slackTimestamp))
                return false;

            request.EnableBuffering();

            using var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0;

            var generatedSignature = SlackSignatureHelper.GenerateSignature(
                _settings.SigningSecret,
                slackTimestamp!,
                body);

            return generatedSignature == slackSignature;
        }
        catch
        {
            return false;
        }
    }

    public async Task HandleEventAsync(SlackEventRequest request)
    {
        if (request.Event == null)
            return;

        if (request.Event.Type != "reaction_added")
            return;

        Console.WriteLine("==================================");
        Console.WriteLine("Reaction Added");
        Console.WriteLine($"Reaction : {request.Event.Reaction}");
        Console.WriteLine($"Channel  : {request.Event.Item?.Channel}");
        Console.WriteLine($"Message  : {request.Event.Item?.Ts}");
        Console.WriteLine("==================================");

        // Only continue for 🐞 reaction
        if (!string.Equals(request.Event.Reaction, "bug", StringComparison.OrdinalIgnoreCase))
            return;

        if (request.Event.Item == null)
            return;

        var payload = SlackBlockBuilder.BuildLogIssueMessage(
            request.Event.Item.Channel!,
            request.Event.Item.Ts!);

        var success = await _slackApiClient.PostThreadMessageAsync(payload);

        Console.WriteLine(success
            ? "Log Issue button posted."
            : "Failed to post Log Issue button.");
    }
}