using SlackJiraAutomation.Api.Slack.Models;

namespace SlackJiraAutomation.Api.Slack.Builders;

public static class JiraModalBuilder
{
    public static JiraModalPayload Build(
        string triggerId,
        string channelId,
        string threadTs,
        string messageTs)
    {
        return new JiraModalPayload
        {
            TriggerId = triggerId,

            View =
            {
                CallbackId = "create_jira",

                // Save Slack information
                PrivateMetadata =
                    $"{channelId}|{threadTs}|{messageTs}",

                Title =
                {
                    Text = "Create Jira"
                },

                Submit =
                {
                    Text = "Create"
                },

                Close =
                {
                    Text = "Cancel"
                },

                Blocks =
                {
                    new JiraBlock
                    {
                        Type = "input",
                        BlockId = "summary",

                        Label =
                        {
                            Text = "Summary"
                        },

                        Element =
                        {
                            ActionId = "summary",

                            Placeholder =
                            {
                                Text = "Issue Summary"
                            }
                        }
                    },

                    new JiraBlock
                    {
                        Type = "input",
                        BlockId = "description",

                        Label =
                        {
                            Text = "Description"
                        },

                        Element =
                        {
                            ActionId = "description",
                            Multiline = true,

                            Placeholder =
                            {
                                Text = "Describe the issue"
                            }
                        }
                    }
                }
            }
        };
    }
}