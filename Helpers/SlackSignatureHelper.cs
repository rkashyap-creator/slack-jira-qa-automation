using System.Security.Cryptography;
using System.Text;

namespace SlackJiraAutomation.Api.Helpers;

public static class SlackSignatureHelper
{
    public static string GenerateSignature(
        string signingSecret,
        string timestamp,
        string body)
    {
        var baseString = $"v0:{timestamp}:{body}";

        using var hmac = new HMACSHA256(
            Encoding.UTF8.GetBytes(signingSecret));

        var hash = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(baseString));

        return "v0=" + Convert.ToHexString(hash).ToLower();
    }
}