using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SlackJiraAutomation.Api.Models;
using SlackJiraAutomation.Api.Slack.Models;

namespace SlackJiraAutomation.Api.Slack.Clients;

public class SlackApiClient
{
    private readonly HttpClient _httpClient;
    private readonly SlackSettings _settings;

    private readonly JsonSerializerOptions _jsonOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true
        };

    public SlackApiClient(
        HttpClient httpClient,
        IOptions<SlackSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;

        _httpClient.BaseAddress =
            new Uri("https://slack.com/api/");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _settings.BotToken);

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    //--------------------------------------------------
    // Test Connection
    //--------------------------------------------------

    public async Task<bool> TestConnectionAsync()
    {
        var response =
            await GetAsync<AuthTestResponse>("auth.test");

        return response?.Ok ?? false;
    }

    //--------------------------------------------------
    // Send Thread Message
    //--------------------------------------------------

    public async Task<bool> PostThreadMessageAsync(
        SlackMessagePayload payload)
    {
        var response =
            await PostAsync<SlackResponse>(
                "chat.postMessage",
                payload);

        if (response == null)
            return false;

        if (!response.Ok)
        {
            Console.WriteLine(
                $"Slack Error : {response.Error}");
        }

        return response.Ok;
    }

    //--------------------------------------------------
    // Open Slack Modal
    //--------------------------------------------------

    public async Task<bool> OpenModalAsync(
        JiraModalPayload payload)
    {
        var response =
            await PostAsync<SlackResponse>(
                "views.open",
                payload);

        if (response == null)
            return false;

        if (!response.Ok)
        {
            Console.WriteLine(
                $"Slack Modal Error : {response.Error}");
        }

        return response.Ok;
    }

    //--------------------------------------------------
    // Generic GET
    //--------------------------------------------------

    private async Task<T?> GetAsync<T>(
        string endpoint)
    {
        var response =
            await _httpClient.GetAsync(endpoint);

        var json =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(json);
            return default;
        }

        return JsonSerializer.Deserialize<T>(
            json,
            _jsonOptions);
    }

    //--------------------------------------------------
    // Generic POST
    //--------------------------------------------------

    private async Task<T?> PostAsync<T>(
        string endpoint,
        object payload)
    {
        var json =
            JsonSerializer.Serialize(
                payload,
                _jsonOptions);

        var content =
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

        var response =
            await _httpClient.PostAsync(
                endpoint,
                content);

        var responseJson =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(responseJson);
            return default;
        }

        return JsonSerializer.Deserialize<T>(
            responseJson,
            _jsonOptions);
    }
}

public class AuthTestResponse
{
    public bool Ok { get; set; }

    public string? Team { get; set; }

    public string? User { get; set; }

    public string? UserId { get; set; }
}

public class SlackResponse
{
    public bool Ok { get; set; }

    public string? Error { get; set; }

    public string? Ts { get; set; }

    public string? Warning { get; set; }
}