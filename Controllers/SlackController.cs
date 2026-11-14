using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SlackJiraAutomation.Api.Interfaces;
using SlackJiraAutomation.Api.Models.Slack;
using SlackJiraAutomation.Api.Slack.Handlers;
using SlackJiraAutomation.Api.Slack.Models;

namespace SlackJiraAutomation.Api.Controllers;

[ApiController]
[Route("api/slack")]
public class SlackController : ControllerBase
{
    private readonly ISlackService _slackService;
    private readonly SlackInteractionHandler _interactionHandler;

    public SlackController(
        ISlackService slackService,
        SlackInteractionHandler interactionHandler)
    {
        _slackService = slackService;
        _interactionHandler = interactionHandler;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var connected = await _slackService.TestConnectionAsync();

        return Ok(new
        {
            Connected = connected
        });
    }

    [HttpPost("events")]
    public async Task<IActionResult> Events([FromBody] SlackEventRequest request)
    {
        // Verify Slack request
        var valid = await _slackService.VerifyRequestAsync(Request);

        if (!valid)
            return Unauthorized();

        // Slack URL verification
        if (request.Type == "url_verification")
        {
            return Ok(new
            {
                challenge = request.Challenge
            });
        }

        // Event callback
        if (request.Type == "event_callback")
        {
            await _slackService.HandleEventAsync(request);
        }

        return Ok();
    }

    [HttpPost("interactions")]
    public async Task<IActionResult> Interactions()
    {
        var valid = await _slackService.VerifyRequestAsync(Request);

        if (!valid)
            return Unauthorized();

        var form = await Request.ReadFormAsync();

        var payload = form["payload"].ToString();

        if (string.IsNullOrWhiteSpace(payload))
            return BadRequest();

        var interaction =
            JsonSerializer.Deserialize<SlackInteractionRequest>(payload);

        if (interaction == null)
            return BadRequest();

        await _interactionHandler.HandleAsync(interaction);

        return Ok();
    }
}