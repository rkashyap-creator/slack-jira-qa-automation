using Serilog;
using SlackJiraAutomation.Api.Interfaces;
using SlackJiraAutomation.Api.Jira.Services;
using SlackJiraAutomation.Api.Models;
using SlackJiraAutomation.Api.Services;
using SlackJiraAutomation.Api.Slack.Clients;
using SlackJiraAutomation.Api.Slack.Handlers;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Logging
// --------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// --------------------------------------------------
// Configuration
// --------------------------------------------------
builder.Services.Configure<SlackSettings>(
    builder.Configuration.GetSection("Slack"));

builder.Services.Configure<JiraSettings>(
    builder.Configuration.GetSection("Jira"));

// --------------------------------------------------
// Http Clients
// --------------------------------------------------
builder.Services.AddHttpClient<SlackApiClient>();

builder.Services.AddHttpClient<IJiraService, JiraService>();

// --------------------------------------------------
// Services
// --------------------------------------------------
builder.Services.AddScoped<ISlackService, SlackService>();

builder.Services.AddScoped<SlackInteractionHandler>();

// --------------------------------------------------
// ASP.NET Core
// --------------------------------------------------
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// --------------------------------------------------
// Build
// --------------------------------------------------
var app = builder.Build();

// --------------------------------------------------
// Middleware
// --------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();