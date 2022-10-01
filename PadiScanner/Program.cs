using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PadiScanner.Components.Geolocation;
using PadiScanner.Data;
using PadiScanner.Infra;
using PadiScanner.Pages.Auth;
using PadiScanner.Pages.Dashboard;
using PadiScanner.Pages.Maps;
using PadiScanner.Pages.Prediction;
using PadiScanner.Pages.Users;
using Serilog;
using System.Reflection;
using System.Security.Claims;

// setup logging
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

// create builder
var builder = WebApplication.CreateBuilder(args);

// load configuration
builder.Configuration
    .AddEnvironmentVariables("PADI_")
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.Development.json", true);

builder.Services.Configure<PadiConfiguration>(builder.Configuration);
var config = builder.Configuration.Get<PadiConfiguration>()!;

// logging
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    var telemetryService = services.GetRequiredService<TelemetryConfiguration>();
    loggerConfig
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(telemetryService, TelemetryConverter.Traces);
});

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddOptions();
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(AppPolicies.Administrator, p => p.RequireClaim(ClaimTypes.Role, UserRole.Administrator.ToString()));
    config.AddPolicy(AppPolicies.Member, p => p.RequireClaim(ClaimTypes.Role, UserRole.Administrator.ToString(), UserRole.Member.ToString()));
    config.AddPolicy(AppPolicies.Guest, p => p.RequireClaim(ClaimTypes.Role, UserRole.Administrator.ToString(), UserRole.Member.ToString(), UserRole.Guest.ToString()));
});

builder.Services.AddDbContext<PadiDataContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<GlobalAppState>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, PadiAuthProvider>();

builder.Services.AddScoped<IAuthViewModel, AuthViewModel>();
builder.Services.AddScoped<IUserViewModel, UserViewModel>();
builder.Services.AddScoped<IDashboardViewModel, DashboardViewModel>();
builder.Services.AddScoped<IPredictionsViewModel, PredictionsViewModel>();
builder.Services.AddScoped<IMapsViewModel, MapsViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
