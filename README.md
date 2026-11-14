# Slack–Jira QA Issue Automation

A .NET 8 Web API that turns a Slack 🐞 reaction into a guided Jira issue workflow.

**Created by Ritik Kashyap.**

## Current workflow

1. A user adds the `:bug:` reaction to a Slack message.
2. Slack sends a signed `reaction_added` event to `POST /api/slack/events`.
3. The bot replies in the message thread with a **Log Issue** button.
4. The button opens a Slack modal for the issue summary and description.
5. Submitting the modal creates a Jira issue and replies in the Slack thread with the Jira link.

## Requirements

- .NET 8 SDK
- A Slack app with a bot token and signing secret
- A Jira Cloud site, account email, API token, and project key
- A public HTTPS endpoint for Slack events and interactions when running outside a local tunnel

## Configuration

Copy `appsettings.example.json` to `appsettings.json` for local development, then supply your own values. Do not commit real credentials. You can also use .NET environment variables:

| Setting | Environment variable |
| --- | --- |
| Slack bot token | `Slack__BotToken` |
| Slack signing secret | `Slack__SigningSecret` |
| Slack app token | `Slack__AppToken` |
| Jira site URL | `Jira__BaseUrl` |
| Jira account email | `Jira__Email` |
| Jira API token | `Jira__ApiToken` |
| Jira project key | `Jira__ProjectKey` |

For local development, .NET User Secrets are another option. Never put working Slack or Jira credentials in source control.

## Run locally

From this directory:

```bash
dotnet restore
dotnet run
```

When the app is running in Development, Swagger is available at `/swagger`. The health endpoint is `/api/health`.

Configure the Slack app's **Event Subscriptions** request URL as `<public-base-url>/api/slack/events` and its **Interactivity & Shortcuts** request URL as `<public-base-url>/api/slack/interactions`. Subscribe the bot to the `reaction_added` event and grant the bot the permissions needed to read reactions, post messages, and open modals. Reinstall the app after changing scopes or event subscriptions.

## Advanced versions and customization

Looking for an advanced or customized version? Contact me to discuss enhancements at a nominal cost: [ritik67500@gmail.com](mailto:ritik67500@gmail.com).

## Important before deployment

This repository contains the starting implementation. Review and secure the diagnostic Jira routes before exposing the API publicly. Confirm Slack signature validation and callback timing against Slack's requirements, then test the full flow in a non-production Slack workspace and Jira project. The app does not include hosting, a database, or a queue.

## Project layout

- `Controllers/` — HTTP endpoints for health checks, Slack events/interactions, and Jira diagnostics
- `Services/` — Slack event handling and Jira API integration
- `Slack/` — Slack API client, interaction handler, modal models, and message builders
- `Jira/` — Jira request and response models
