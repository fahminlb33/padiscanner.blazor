using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PadiScanner.Components.Geolocation;
using PadiScanner.Data;
using PadiScanner.Infra;
using PadiScanner.Infra.Services;
using PadiScanner.Pages.Auth;
using PadiScanner.Pages.Dashboard;
using PadiScanner.Pages.Maps;
using PadiScanner.Pages.Prediction;
using PadiScanner.Pages.Users;
using Serilog;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;

// setup logging
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

// create builder
var builder = WebApplication.CreateBuilder(args);

// load configuration
builder.Configuration.AddEnvironmentVariables("PADI_");
builder.Services.Configure<PadiConfiguration>(builder.Configuration);

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

// add EF Core
builder.Services.AddDbContext<PadiDataContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add authorization
builder.Services.AddScoped<AuthenticationStateProvider, PadiAuthProvider>();
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(AppPolicies.Administrator, p => p.RequireClaim(ClaimTypes.Role, UserRole.Administrator.ToString()));
    config.AddPolicy(AppPolicies.Member, p => p.RequireClaim(ClaimTypes.Role, UserRole.Administrator.ToString(), UserRole.Member.ToString()));
    config.AddPolicy(AppPolicies.Guest, p => p.RequireClaim(ClaimTypes.Role, UserRole.Administrator.ToString(), UserRole.Member.ToString(), UserRole.Guest.ToString()));
});

// add automapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// add app services
builder.Services.AddScoped<GlobalAppState>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

builder.Services.AddHttpClient<IImageAnalysisService, ImageAnalysisService>(x =>
{
    var config = builder.Configuration.Get<PadiConfiguration>();
    var credString = $"{config.AnalysisApi.Username}:{config.AnalysisApi.Password}";
    var base64Cred = Convert.ToBase64String(Encoding.ASCII.GetBytes(credString));
    
    x.BaseAddress = new Uri(config.AnalysisApi.Host);
    x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Cred);
    x.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("PadiScanner", "1.0"));
});

// add background service
builder.Services.AddHostedService<PredictionBackgroundService>();

// add view models
builder.Services.AddScoped<IAuthViewModel, AuthViewModel>();
builder.Services.AddScoped<IUserViewModel, UserViewModel>();
builder.Services.AddScoped<IMapsViewModel, MapsViewModel>();
builder.Services.AddScoped<IDashboardViewModel, DashboardViewModel>();
builder.Services.AddScoped<IPredictionsViewModel, PredictionsViewModel>();

// build app
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

// bootstrap app
try
{
    Log.Information("Starting web host");

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return -1;
}
finally
{
    Log.CloseAndFlush();
}

