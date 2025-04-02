using Services;
using Nodes;
using Middleware;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

// Configure structured logging
builder.Logging.ClearProviders();
builder.Services.Configure<ConsoleFormatterOptions>(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "[HH:mm:ss] ";
});

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KubernetesServices>();
builder.Services.AddSingleton<KubernetesNodes>();
builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

// Enable CORS middleware
app.UseCors("AllowAllOrigins");

// Add global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
