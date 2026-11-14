namespace SlackJiraAutomation.Api.Slack.Models;

public class SlackMessagePayload
{
    public string Channel { get; set; } = string.Empty;

    public string ThreadTs { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public List<SlackBlock> Blocks { get; set; } = new();
}

public class SlackBlock
{
    public string Type { get; set; } = string.Empty;

    public SlackText? Text { get; set; }

    public List<SlackButton>? Elements { get; set; }
}

public class SlackText
{
    public string Type { get; set; } = "mrkdwn";

    public string Text { get; set; } = string.Empty;
}

public class SlackButton
{
    public string Type { get; set; } = "button";

    public SlackButtonText Text { get; set; } = new();

    public string Style { get; set; } = "primary";

    public string ActionId { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}

public class SlackButtonText
{
    public string Type { get; set; } = "plain_text";

    public string Text { get; set; } = string.Empty;
}