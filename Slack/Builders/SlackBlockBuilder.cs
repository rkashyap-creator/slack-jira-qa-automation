using SlackJiraAutomation.Api.Slack.Models;

namespace SlackJiraAutomation.Api.Slack.Builders;

public static class SlackBlockBuilder
{
    public static SlackMessagePayload BuildLogIssueMessage(
        string channelId,
        string threadTs)
    {
        return new SlackMessagePayload
        {
            Channel = channelId,
            ThreadTs = threadTs,
            Text = "Create Jira Issue",

            Blocks = new List<SlackBlock>
            {
                new SlackBlock
                {
                    Type = "section",
                    Text = new SlackText
                    {
                        Type = "mrkdwn",
                        Text = "🐞 *Create Jira Issue*"
                    }
                },

                new SlackBlock
                {
                    Type = "actions",
                    Elements = new List<SlackButton>
                    {
                        new SlackButton
                        {
                            ActionId = "log_issue",
                            Value = "create_jira",
                            Style = "primary",
                            Text = new SlackButtonText
                            {
                                Type = "plain_text",
                                Text = "Log Issue"
                            }
                        }
                    }
                }
            }
        };
    }

    public static SlackMessagePayload BuildJiraCreatedMessage(
        string channelId,
        string threadTs,
        string issueKey,
        string browseUrl)
    {
        return new SlackMessagePayload
        {
            Channel = channelId,
            ThreadTs = threadTs,
            Text =
$@"✅ *Jira Issue Created Successfully*

*Issue:* {issueKey}

{browseUrl}"
        };
    }

    public static SlackMessagePayload BuildJiraFailedMessage(
        string channelId,
        string threadTs)
    {
        return new SlackMessagePayload
        {
            Channel = channelId,
            ThreadTs = threadTs,
            Text =
@"❌ Failed to create Jira issue.

Please check server logs."
        };
    }
}